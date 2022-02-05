#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTTT.Ejemplo.Linq.Data.Entity;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Collections;
using UTTT.Ejemplo.Persona.Control;
using UTTT.Ejemplo.Persona.Control.Ctrl;
using System.Net.Mail;
using System.Threading;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

#endregion

namespace UTTT.Ejemplo.Persona
{
    public partial class PersonaManager : System.Web.UI.Page
    {
        #region Variables

        private SessionManager session = new SessionManager();
        private int idPersona = 0;
        private UTTT.Ejemplo.Linq.Data.Entity.Persona baseEntity;
        private DataContext dcGlobal = new DcGeneralDataContext();
        private int tipoAccion = 0;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Response.Buffer = true;
                this.session = (SessionManager)this.Session["SessionManager"];
                this.idPersona = this.session.Parametros["idPersona"] != null ?
                    int.Parse(this.session.Parametros["idPersona"].ToString()) : 0;
                if (this.idPersona == 0)
                {
                    this.baseEntity = new Linq.Data.Entity.Persona();
                    this.tipoAccion = 1;
                }
                else
                {
                    this.baseEntity = dcGlobal.GetTable<Linq.Data.Entity.Persona>().First(c => c.id == this.idPersona);
                    this.tipoAccion = 2;
                }

                if (!this.IsPostBack)
                {
                    if (this.session.Parametros["baseEntity"] == null)
                    {
                        this.session.Parametros.Add("baseEntity", this.baseEntity);
                    }
                    List<CatSexo> lista = dcGlobal.GetTable<CatSexo>().ToList();
                    CatSexo catTemp = new CatSexo();
                    catTemp.id = -1;
                    catTemp.strValor = "Seleccionar";
                    lista.Insert(0, catTemp);
                    this.ddlSexo.DataTextField = "strValor";
                    this.ddlSexo.DataValueField = "id";
                    this.ddlSexo.DataSource = lista;
                    this.ddlSexo.DataBind();

                    this.ddlSexo.SelectedIndexChanged += new EventHandler(ddlSexo_SelectedIndexChanged);
                    this.ddlSexo.AutoPostBack = true;
                    if (this.idPersona == 0)
                    {
                        this.lblAccion.Text = "Agregar";
                    }
                    else
                    {
                        this.lblAccion.Text = "Editar";
                        this.txtNombre.Text = this.baseEntity.strNombre;
                        this.txtAPaterno.Text = this.baseEntity.strAPaterno;
                        this.txtAMaterno.Text = this.baseEntity.strAMaterno;
                        this.txtClaveUnica.Text = this.baseEntity.strClaveUnica;
                        this.txtCURP.Text = this.baseEntity.strCurp;
                        this.setItem(ref this.ddlSexo, baseEntity.CatSexo.strValor);
                    }                
                }

            }
            catch (Exception _e)
            {
                this.showMessage("Ha ocurrido un problema al cargar la página");
                this.Response.Redirect("~/PersonaPrincipal.aspx", false);
            }

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                rvClaveUnica.Validate();
                revNombre.Validate();
                revAPaterno.Validate();
                revAMaterno.Validate();
                revCURP.Validate();
                rfvNombre.Validate();
                rfvAPaterno.Validate();
                rfvAMaterno.Validate();
                rfvCurp.Validate();

                //if (!Page.IsValid)
                //{
                //    return;
                //}
                DataContext dcGuardar = new DcGeneralDataContext();
                UTTT.Ejemplo.Linq.Data.Entity.Persona persona = new Linq.Data.Entity.Persona();
                if (this.idPersona == 0)
                {
                  

                    persona.strClaveUnica = this.txtClaveUnica.Text.Trim();
                    persona.strNombre = this.txtNombre.Text.Trim();
                    persona.strAMaterno = this.txtAMaterno.Text.Trim();
                    persona.strAPaterno = this.txtAPaterno.Text.Trim();
                    persona.strCurp = this.txtCURP.Text.Trim();
                    persona.idCatSexo = int.Parse(this.ddlSexo.Text);

                    String mensaje = String.Empty;
                    if (this.vistaBacia(persona))
                    {

                        this.regresar();
                    }

                    if (!this.validacion(persona, ref mensaje))
                    {
                        ////Validacion de datos correctos desde código
                        this.lblMensaje.Text = mensaje;
                        this.lblMensaje.Visible = true;
                        return;
                    }

              

                    dcGuardar.GetTable<UTTT.Ejemplo.Linq.Data.Entity.Persona>().InsertOnSubmit(persona);
                    dcGuardar.SubmitChanges();
                    this.showMessage("El registro se agrego correctamente.");
                    this.Response.Redirect("~/PersonaPrincipal.aspx", false);

                }
                if (this.idPersona > 0)
                {
                    persona = dcGuardar.GetTable<UTTT.Ejemplo.Linq.Data.Entity.Persona>().First(c => c.id == idPersona);
                    persona.strClaveUnica = this.txtClaveUnica.Text.Trim();
                    persona.strNombre = this.txtNombre.Text.Trim();
                    persona.strAMaterno = this.txtAMaterno.Text.Trim();
                    persona.strAPaterno = this.txtAPaterno.Text.Trim();
                    persona.strCurp = this.txtCURP.Text.Trim();
                    persona.idCatSexo = int.Parse(this.ddlSexo.Text);
                    dcGuardar.SubmitChanges();
                    this.showMessage("El registro se edito correctamente.");
                    this.Response.Redirect("~/PersonaPrincipal.aspx", false);
                }
            }
            catch (Exception _e)
            {
                this.showMessageException(_e.Message);
                          

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {              
                this.Response.Redirect("~/PersonaPrincipal.aspx", false);
            }
            catch (Exception _e)
            {
                this.showMessage("Ha ocurrido un error inesperado");
            }
        }

        protected void ddlSexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idSexo = int.Parse(this.ddlSexo.Text);
                Expression<Func<CatSexo, bool>> predicateSexo = c => c.id == idSexo;
                predicateSexo.Compile();
                List<CatSexo> lista = dcGlobal.GetTable<CatSexo>().Where(predicateSexo).ToList();
                CatSexo catTemp = new CatSexo();            
                this.ddlSexo.DataTextField = "strValor";
                this.ddlSexo.DataValueField = "id";
                this.ddlSexo.DataSource = lista;
                this.ddlSexo.DataBind();
            }
            catch (Exception)
            {
                this.showMessage("Ha ocurrido un error inesperado");
            }
        }

        #endregion

        #region Metodos

        public void setItem(ref DropDownList _control, String _value)
        {
            foreach (ListItem item in _control.Items)
            {
                if (item.Value == _value)
                {
                    item.Selected = true;
                    break;
                }
            }
            _control.Items.FindByText(_value).Selected = true;
        }

        #endregion

        #region Validación Código
        ///<summary>
        ///Valida datos básicos
        ///</summary>
        ///<param name="_persona"></param>
        ///<param name="_mensaje"></param>
        ///<returns></returns>

        public bool validacion(UTTT.Ejemplo.Linq.Data.Entity.Persona _persona, ref String _mensaje)
        {
            if(_persona.idCatSexo == -1)
            {
                _mensaje = "Seleccione Masculino o Femenino";
                return false;
            }
            int i = 0;
            //Verificar si un texto es un número
            if(int.TryParse(_persona.strClaveUnica, out i) == false)
            {
                _mensaje = "La Clave Unica no es un número";
                return false;
            }
            ////Validamos un número
            ////string, saber que es un número
            ////99 y 1000
           if(int.Parse(_persona.strClaveUnica) < 100 || int.Parse(_persona.strClaveUnica) > 999)
            {
                _mensaje = "La Clave Unica esta fuera de rango";
                return false;
            }
            if (_persona.strNombre.Equals(String.Empty))
            {
                _mensaje = "El campo Nombre está vacio";
                return false;
            }
            if (_persona.strNombre.Length > 50)
            {
                _mensaje = "Los caracteres permitidos para nombre rebasan lo establecido de 50";
                return false;
            }
            if (_persona.strNombre.Length < 3)
            {
                _mensaje = "Ingrese un Nombre real";
                return false;
            }

            if (_persona.strAPaterno.Equals(String.Empty))
            {
                _mensaje = "El campo APaterno esta vacio";
                return false;
            }

            if (_persona.strAPaterno.Length > 50)
            {
                _mensaje = "Los caracteres permitidos para nombre rebasan lo establecido de 50 para A Paterno";
                return false;
            }
            if (_persona.strAPaterno.Length < 3)
            {
                _mensaje = "Ingrese un Apellido Real";
                return false;
            }
            if (_persona.strAMaterno.Equals(String.Empty))
            {
                _mensaje = "El campo AMaterno esta vacio";
                return false;
            }

            if (_persona.strAMaterno.Length > 50)
            {
                _mensaje = "Los caracteres permitidos para nombre rebasan lo establecido de 50 para A Materno";
                return false;
            }
            if (_persona.strAMaterno.Length < 3)
            {
                _mensaje = "Ingrese un Apellido Real";
                return false;
            }
            if (_persona.strCurp.Equals(String.Empty))
            {
                _mensaje = "El campo Curp esta vacio";
                return false;
            }

            if (_persona.strCurp.Length > 50)
            {
                _mensaje = "Los caracteres permitidos para Curp rebasan lo establecido de 50";
                return false;
            }
            if (_persona.strCurp.Length < 18)
            {
                _mensaje = "Los caracteres permitidos para Curp tiene que ser 18";
                return false;
            }
            return true;

        }
        public bool vistaBacia(UTTT.Ejemplo.Linq.Data.Entity.Persona _persona)
        {

            try
            {
                if (_persona.idCatSexo == -1 && _persona.strClaveUnica.Equals(string.Empty) && _persona.strNombre.Equals(string.Empty)
                    && _persona.strAPaterno.Equals(string.Empty) && _persona.strAMaterno.Equals(string.Empty) &&
                       _persona.strCurp.Equals(string.Empty))
                {
                    return true;
                }
                else
                {

                    return false;

                }

            }
            catch (Exception e)
            {
               

            }
            return false;
        }

        public void regresar()
        {
            this.Response.Redirect("~/PersonaPrincipal.aspx", false);

        }
        #endregion

        protected void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }
    }
}