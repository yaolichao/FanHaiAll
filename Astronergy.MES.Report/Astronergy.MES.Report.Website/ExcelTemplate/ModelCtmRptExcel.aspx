<%@ Page Language="C#" MasterPageFile="~/Master/MasterPage.master" AutoEventWireup="true" CodeFile="ModelCtmRptExcel.aspx.cs" Inherits="ExcelTemplate_ModelCtmRptExcel" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="Server" Width="960px" ForeColor="#333333" GridLines="Both"
        CellPadding="0" CellSpacing="0" AutoGenerateColumns="false" 
        Font-Size="9pt" onrowdatabound="GridView1_RowDataBound">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle Font-Bold="True" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Font-Size="9pt" />
        <PagerStyle ForeColor="Black" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
         <Columns>
               <asp:TemplateField HeaderText="序号">
                    <ItemTemplate>
                        <asp:Label ID="lblt_no" runat="server" Text='<%# Bind("NO") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="日期">
                    <ItemTemplate>
                        <asp:Label ID="lblt_date" runat="server" Text='<%# Bind("T_DATE") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="测试时间">
                    <ItemTemplate>
                        <asp:Label ID="lblttime" runat="server" Text='<%# Bind("TTIME") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="条码">
                    <ItemTemplate>
                        <asp:Label ID="lblserialno" runat="server" Text='<%# Bind("LOT_NUM") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="产品型号">
                    <ItemTemplate>
                        <asp:Label ID="lblVoc" runat="server" Text='<%# Bind("PRO_ID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="测试温度(℃)">
                    <ItemTemplate>
                        <asp:Label ID="lblambienttemp" runat="server" Text='<%# Bind("AMBIENTTEMP") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="光强">
                    <ItemTemplate>
                        <asp:Label ID="lblIntensity" runat="server" Text='<%# Bind("INTENSITY") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="填充因子(%)">
                    <ItemTemplate>
                        <asp:Label ID="lblff" runat="server" Text='<%# Bind("FF") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="组件转换效率(%)">
                    <ItemTemplate>
                        <asp:Label ID="lbleff" runat="server" Text='<%# Bind("EFF") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Maximum Power">
                    <ItemTemplate>
                        <asp:Label ID="lblpm" runat="server" Text='<%# Bind("PM") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Isc">
                    <ItemTemplate>
                        <asp:Label ID="lblJunctionBox" runat="server" Text='<%# Bind("ISC") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Imp">
                    <ItemTemplate>
                        <asp:Label ID="lblisc" runat="server" Text='<%# Bind("IPM") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voc">
                    <ItemTemplate>
                        <asp:Label ID="lblvoc" runat="server" Text='<%# Bind("VOC") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vpm">
                    <ItemTemplate>
                        <asp:Label ID="lblvpm" runat="server" Text='<%# Bind("VPM") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="设备编码">
                    <ItemTemplate>
                        <asp:Label ID="lbldevicenum" runat="server" Text='<%# Bind("DEVICENUM") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="打印标志">
                    <ItemTemplate>
                        <asp:Label ID="lblpsing" runat="server" Text='<%# Bind("VC_PSIGN") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="打印时间">
                    <ItemTemplate>
                        <asp:Label ID="lblprintdt" runat="server" Text='<%# Bind("DT_PRINTDT") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="打印次数">
                    <ItemTemplate>
                        <asp:Label ID="lblpnum" runat="server" Text='<%# Bind("P_NUM") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="有效数据">
                    <ItemTemplate>
                        <asp:Label ID="lbldefault" runat="server" Text='<%# Bind("VC_DEFAULT") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="环境温度(℃)">
                    <ItemTemplate>
                        <asp:Label ID="lblsensortemp" runat="server" Text='<%# Bind("SENSORTEMP") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="客户编码">
                    <ItemTemplate>
                        <asp:Label ID="lblcustode" runat="server" Text='<%# Bind("VC_CUSTCODE") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作员">
                    <ItemTemplate>
                        <asp:Label ID="lbluserid" runat="server" Text='<%# Bind("C_USERID") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="衰减最大功率">
                    <ItemTemplate>
                        <asp:Label ID="lblcoefpmax" runat="server" Text='<%# Bind("COEF_PMAX") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="衰减短路电流">
                    <ItemTemplate>
                        <asp:Label ID="lblcoefisc" runat="server" Text='<%# Bind("COEF_ISC") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="衰减开路电压">
                    <ItemTemplate>
                        <asp:Label ID="lblcoefvoc" runat="server" Text='<%# Bind("COEF_VOC") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="衰减最大工作电流">
                    <ItemTemplate>
                        <asp:Label ID="lblcoefimax" runat="server" Text='<%# Bind("COEF_IMAX") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="衰减最大工作电压">
                    <ItemTemplate>
                        <asp:Label ID="lblcoefvmax" runat="server" Text='<%# Bind("COEF_VMAX") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="衰减填充因子">
                    <ItemTemplate>
                        <asp:Label ID="lblcoefff" runat="server" Text='<%# Bind("COEF_FF") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="电池片效率">
                    <ItemTemplate>
                        <asp:Label ID="lblcelleff" runat="server" Text='<%# Bind("VC_CELLEFF") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CTM">
                    <ItemTemplate>
                        <asp:Label ID="lblctm" runat="server" Text='<%# Bind("DEC_CTM") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="工单号">
                    <ItemTemplate>
                        <asp:Label ID="lblworkorder" runat="server" Text='<%# Bind("VC_WORKORDER") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rs">
                    <ItemTemplate>
                        <asp:Label ID="lblrs" runat="server" Text='<%# Bind("RS") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rsh">
                    <ItemTemplate>
                        <asp:Label ID="lblrsh" runat="server" Text='<%# Bind("RSH") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="校准板序号">
                    <ItemTemplate>
                        <asp:Label ID="lblcalibrationno" runat="server" Text='<%# Bind("CALIBRATION_NO") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="电池片信息">
                    <ItemTemplate>
                        <asp:Label ID="lblsilot" runat="server" Text='<%# Bind("SI_LOT") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="单焊长焊带">
                    <ItemTemplate>
                        <asp:Label ID="lblpn1" runat="server" Text='<%# Bind("PN1") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="单焊短焊带">
                    <ItemTemplate>
                        <asp:Label ID="lblpn2" runat="server" Text='<%# Bind("PN2") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="串焊短焊带">
                    <ItemTemplate>
                        <asp:Label ID="lblpn3" runat="server" Text='<%# Bind("PN3") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="助焊剂">
                    <ItemTemplate>
                        <asp:Label ID="lblpn4" runat="server" Text='<%# Bind("PN4") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="汇流条">
                    <ItemTemplate>
                        <asp:Label ID="lblpn5" runat="server" Text='<%# Bind("PN5") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="玻璃">
                    <ItemTemplate>
                        <asp:Label ID="lblpn6" runat="server" Text='<%# Bind("PN6") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="上层EVA">
                    <ItemTemplate>
                        <asp:Label ID="lblpn7" runat="server" Text='<%# Bind("PN7") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="下层EVA">
                    <ItemTemplate>
                        <asp:Label ID="lblpn8" runat="server" Text='<%# Bind("PN8") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="背板">
                    <ItemTemplate>
                        <asp:Label ID="lblpn9" runat="server" Text='<%# Bind("PN9") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="线盒">
                    <ItemTemplate>
                        <asp:Label ID="lblpn10" runat="server" Text='<%# Bind("PN10") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="层压时间">
                    <ItemTemplate>
                        <asp:Label ID="lblstarttime" runat="server" Text='<%# Bind("START_TIMESTAMP") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="层压机编号">
                    <ItemTemplate>
                        <asp:Label ID="lblequipmentcode" runat="server" Text='<%# Bind("EQUIPMENT_CODE") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle />
                    <HeaderStyle />
                </asp:TemplateField>
          </Columns>
         <EmptyDataTemplate>
            No Data...
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>

