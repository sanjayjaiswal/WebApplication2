﻿<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApplication2.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Your application description page.</h3>
    <p>Use this area to provide additional information.</p>
    
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" /><br />
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
</asp:Content>
