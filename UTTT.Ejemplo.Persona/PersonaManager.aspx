<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonaManager.aspx.cs" Inherits="UTTT.Ejemplo.Persona.PersonaManager" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.4.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>

    <script type="text/javascript">

        function validaNumeros(evt) {
            //valida que solo se ingresen numeros en la caja de texto
            var code = (evt.wich) ? evt.wich : evt.keyCode;
            if (code == 8) {
                return true;
            } else if (code >= 48 && code <= 57) {
                return true;
            } else {
                return false;
            }
        }

        function validaLetras(e) {
            //valida que solo ingrese letras y algunos caracteres especiales
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
            if (!regex.test(key)) {
                e.preventDefault();
                return false;

            }
        }

        function validaCurp(curp) {
            //valida letras y numeros pero si caracteres especiales ya que se trata del CURP
            var regex = new RegExp("^[a-zA-Z0-9 ]+$");
            var key = String.fromCharCode(!curp.charCode ? curp.which : curp.charCode);
            if (!regex.test(key)) {
                curp.preventDefault();
                return false;
            }
        }

    </script>
</head>
<body>

    <div class="col-md-12">
        <nav class="navbar navbar-light" style="background-color: #b1d7f2;">
            <div class="container-fluid">
                <span class="navbar-text mb-0 h1">Persona</span>
            </div>
        </nav>
    </div>
    <section class="container-fluid">
               <form id="form1" runat="server">
                   <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
            </asp:ScriptManager>
     
           <div class="row">
                <div class="col-6">
                    <center>
                        <asp:Label ID="lblAccion" runat="server" Text="Accion" Font-Bold="True"></asp:Label>
                        <p></p>
                    </center>
                </div>
                <div class="col col-6"></div>
          </div>

                   <div class="container">
                <div class="row">
                    <div class="col-2">Sexo:</div>
                    <div class="col-2">
                        <asp:UpdatePanel ID="UpDatePanel1"  runat="server"> 
                            <ContentTemplate> 
                        <asp:DropDownList
                            ID="ddlSexo" class="btn btn-outline-dark" runat="server"
                            Width="210px"
                            OnSelectedIndexChanged="ddlSexo_SelectedIndexChanged">
                        </asp:DropDownList>
                          
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlSexo" EventName="SelectedIndexChanged" />
                        </Triggers>
                        </asp:UpdatePanel>  
                        </div>
                    <div class="col-12 col-xl-8">
                        <asp:RequiredFieldValidator ID="rfvSexo" runat="server" ControlToValidate="ddlSexo" ErrorMessage="*Selecciona el sexo" InitialValue="-1" ValidationGroup="vgvVali"></asp:RequiredFieldValidator>
             
                        </div>
                         <div class="col-2">Clave Unica: </div>
                    <div class="col-2">
                        <asp:TextBox ID="txtClaveUnica" runat="server"
                            Width="210px" ViewStateMode="Disabled"
                            onkeypress="return validaNumeros(event);"  pattern=".{1,3}" >
                        </asp:TextBox>
                    </div>
                    <div class="col-12 col-xl-8">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RangeValidator
                            ID="rvClaveUnica" runat="server" ControlToValidate="txtClaveUnica" ErrorMessage="*La clave única deberá de estar entre 1 y 999"
                            MaximumValue="999" MinimumValue="100" Type="Integer">
                        </asp:RangeValidator>
                    </div>
                   

                         <div class="col-2">Nombre: </div>
                    <div class="col-2">
                        <asp:TextBox 
                            ID="txtNombre" runat="server" Width="210px" ViewStateMode="Disabled" OnTextChanged="txtNombre_TextChanged" pattern=".{3,50}"
                            onkeypress="return validaLetras(event)" >
                        </asp:TextBox>
                    </div>
                    <div class="col-8">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                          <asp:RegularExpressionValidator ID="revNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="*Incluir solamente letras y espacios" ValidationExpression="[a-zA-Z ]{2,254}"></asp:RegularExpressionValidator>
                          <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="*Nombre obligatorio"></asp:RequiredFieldValidator>
                    </div>

                     <div class="col-2">A Paterno: </div>
                    <div class="col-2">
                        <asp:TextBox 
                        ID="txtAPaterno" runat="server" Width="210px" ViewStateMode="Disabled" pattern=".{3,50}"
                        onkeypress="return validaLetras(event);">
                        </asp:TextBox>
                    </div>
                    <div class="col-8">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RegularExpressionValidator ID="revAPaterno" runat="server" ControlToValidate="txtAPaterno" ErrorMessage="*Incluir solamente letras y espacios" ValidationExpression="[a-zA-Z ]{2,254}"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvAPaterno" runat="server" ControlToValidate="txtAPaterno" ErrorMessage="*Apellido Paterno obligatorio"></asp:RequiredFieldValidator>>
                    </div>

                         <div class="col-2">A Materno: </div>
                    <div class="col-2">
                       <asp:TextBox ID="txtAMaterno" runat="server" Width="210px" 
                ViewStateMode="Disabled" pattern=".{3,50}"
                 onkeypress="return validaLetras(event);"
                ></asp:TextBox>
                    </div>
                    <div class="col-8">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RegularExpressionValidator ID="revAMaterno" runat="server" ControlToValidate="txtAMaterno" ErrorMessage="Incluir solamente letras y espacios" ValidationExpression="[a-zA-Z ]{2,254}"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="rfvAMaterno" runat="server" ControlToValidate="txtAMaterno" ErrorMessage="*Apellido Materno obligatorio"></asp:RequiredFieldValidator>
                    </div>

                        <div class="col-2">CURP: </div>
                    <div class="col-2">
                      <asp:TextBox ID="txtCURP" runat="server" MaxLength="18" Width="210px"
                      onkeypress="return validaCurp(event);">
                      </asp:TextBox>
                    </div>
                    <div class="col-12 col-xl-8">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:RegularExpressionValidator ID="revCURP" runat="server" ControlToValidate="txtCURP" ErrorMessage="*La CURP es incorrecta" ValidationExpression="^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvCurp" runat="server" ControlToValidate="txtCURP" ErrorMessage="*Curp obligatorio"></asp:RequiredFieldValidator>
                    </div>

                         <div class="col-2">Fecha de Nacimiento: </div>
                    <div class="col-2">
                      <asp:TextBox ID="txtFechaNacimiento" runat="server" MaxLength="18" Width="210px"
                      onkeypress="return validaCurp(event);">
                      </asp:TextBox>
                        </div>
                        <div class="col-5 col-lg-3 col-xl-1">
                            <center>
                         <asp:ImageButton ID="imgPopup" ImageUrl="https://www.seekpng.com/png/detail/206-2062217_calendario-png.png" ImageAlign="Bottom"
                        runat="server" CausesValidation="False" Width="30px" Height="30px" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" PopupButtonID="imgPopup" Format="dd/MM/yyyy"
                        runat="server" TargetControlID="txtFechaNacimiento"/>
                                </center>
                    </div>
               </div>


         <p><br /></p>
                <div class="row">
                    <div class="col-4">
                        <center>
                            <asp:Button ID="btnAceptar" class="btn btn-outline-primary" runat="server" Text="Aceptar"
                            OnClick="btnAceptar_Click" ViewStateMode="Disabled"  CausesValidation="false"/>

                        <asp:Button ID="btnCancelar" class="btn btn-outline-info" runat="server" Text="Cancelar" CausesValidation="false"
                            OnClick="btnCancelar_Click" ViewStateMode="Disabled" />
                        </center>
                    </div>
                    <div class="col-8">
                        <asp:Label ID="lblMensaje" runat="server" ForeColor="#CC0000" Text="..." Visible="False"></asp:Label>
                    </div>
                </div>
         </div>
    </form>
 </section>
</body>
</html>
