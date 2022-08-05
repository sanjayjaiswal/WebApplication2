<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>
   

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   <%-- <script>
        alert('hi');
    </script>--%>
    <%--<asp:Timer ID="displayTimer" runat="server" Interval="10000" OnTick="displayTimer_Tick"></asp:Timer>--%>
    <%--<asp:Timer ID="displayTimer" runat="server" Interval="10000"></asp:Timer>--%>
    
   
    <table>
        <tr>
            <td>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Start" OnClick="Button1_Click" /></td>
            <td><asp:Button ID="Button2" runat="server" Text="Stop" OnClick="Button2_Click" /></td>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </tr>
    </table>

</asp:Content>
