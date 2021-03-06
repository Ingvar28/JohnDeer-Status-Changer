﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using teemtalk;
using System.Globalization;

namespace Status_changer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static teemtalk. Application teemApp;

        public string EventDepot { get; private set; }

        private void btnStart_Click(object sender, EventArgs e)
        {

            var login = Properties.Settings.Default.loginMF;
            var password = Properties.Settings.Default.pwdMF;
            var consData = DBContext.GetConsStatus();
            teemApp = new teemtalk.Application();
            
            teemApp.CurrentSession.Name = "Mainframe";

            teemApp.CurrentSession.Network.Protocol = ttNetworkProtocol.ProtocolWinsock;
            teemApp.CurrentSession.Network.Hostname = "mainframe.gb.tntpost.com";
            teemApp.CurrentSession.Network.Telnet.Port = 23;
            teemApp.CurrentSession.Network.Telnet.Name = "IBM-3278-2-E";
            teemApp.CurrentSession.Emulation = ttEmulations.IBM3270Emul;

            teemApp.CurrentSession.Network.Connect();

            teemApp.Visible = Properties.Settings.Default.isVisible; ;

            var host = teemApp.CurrentSession.Host;
            var disp = teemApp.CurrentSession.Display;

            ForAwait(35, 16, "INTERNATIONAL");

            host.Send("SM");
            host.Send("<ENTER>");

            ForAwait(13, 23, "USER ID");

            host.Send(login);
            host.Send("<TAB>");
            host.Send(password);
            host.Send("<ENTER>");


            //if (!ForAwait(2, 2, "Command")) goto StartMaimframe;
            ForAwait(2, 2, "Command");
            host.Send("2");
            host.Send("<ENTER>");

            ForAwait(20, 7, "Job Description");
            host.Send("<F12>");
            if (disp.CursorRow != 2)

            host.Send("YL30");
            host.Send("<ENTER>");

            foreach (DataRow row in consData.Rows)
            {
                var qty = "1";
                int id = Convert.ToInt32(row["id"].ToString());
                var con = row["Consignment"].ToString();
                DateTime dateFromBase = row.Field<DateTime>("Date");
                var date = dateFromBase.ToString("ddMMMyy", CultureInfo.GetCultureInfo("en-us"));
                var time = dateFromBase.ToString("HHmm");
                var status = row["Code"].ToString();
                var comment = row["Commentary"].ToString();
                var eventdepot = row["EventDepot"].ToString();  //дописал
                if (comment.Trim() == "") comment = "...";
                
                ForAwait(15, 2, "Consignment Status Entry");

                host.Send(status);
                Thread.Sleep(2000); //костыль
                if (disp.CursorCol != 28 && disp.CursorCol != 10) host.Send("<TAB>");
                ForAwaitCol(28);

                host.Send(date);                
                host.Send("<TAB>");
                ForAwaitCol(46);

                host.Send(time);
                if (disp.CursorCol != 70 && disp.CursorCol != 46) host.Send("<TAB>");
                ForAwaitCol(70);

                host.Send(eventdepot); //поменял
                host.Send("<TAB>");
                ForAwaitCol(13);
                host.Send("<TAB>");
                ForAwaitCol(57);

                //host.Send(date);
                host.Send("<TAB>");
                ForAwaitCol(77);

                host.Send("<F4>");
                ForAwait(5, 5, "Seq Remarks");
                host.Send(comment);

                host.Send("<ENTER>");
                ForAwaitCol(9);
                host.Send("<F12>");
                ForAwaitCol(18);
                host.Send("<F12>");
                ForAwait(15, 2, "Consignment Status Entry");

                host.Send("<TAB>");
                ForAwaitCol(12);
                host.Send("<TAB>");
                ForAwaitCol(33);
                host.Send("<TAB>");
                ForAwaitCol(54);
                host.Send("<TAB>");
                ForAwaitCol(73);
                host.Send("<TAB>");
                ForAwaitCol(24);

                host.Send(qty);
                host.Send("<ENTER>");
                ForAwait(1, 10, "01");

                host.Send(con);
                host.Send("<ENTER>");
                ForAwaitRow(22);
                host.Send("<F12>");
                ForAwait(2, 23, "HIT ENTER");
                host.Send("<ENTER>");
                Thread.Sleep(2000);

                if (disp.ScreenData[15,2,9] == "Duplicate")
                {
                    var checkDepo = "";
                    short i = 1;
                    do
                    {
                        short col = (Int16)(9 + i);
                        checkDepo = disp.ScreenData[54, col, 3];
                        if (checkDepo == "MW3" || checkDepo == "MW5" || checkDepo == "MW7" || checkDepo == "MOW"
                            
                            || checkDepo == "LED"
                            || checkDepo == "KG7"
                            || checkDepo == "GOJ"
                            || checkDepo == "KUF"
                            || checkDepo == "KZ7"
                            || checkDepo == "RO8"
                            || checkDepo == "KR4"
                            || checkDepo == "SVX"
                            || checkDepo == "IK3"
                            || checkDepo == "OVB"
                            || checkDepo == "KH6"
                            || checkDepo == "VK3"
                            || checkDepo == "AB7"
                            || checkDepo == "AC8"
                            || checkDepo == "AK7"
                            || checkDepo == "AP6"
                            || checkDepo == "AV8"
                            || checkDepo == "BA8"
                            || checkDepo == "BB8"
                            || checkDepo == "BG8"
                            || checkDepo == "BU8"
                            || checkDepo == "BY5"
                            || checkDepo == "CB2"
                            || checkDepo == "CT6"
                            || checkDepo == "EL6"
                            || checkDepo == "IV6"
                            || checkDepo == "IZ8"
                            || checkDepo == "JA5"
                            || checkDepo == "KE5"
                            || checkDepo == "KG5"
                            || checkDepo == "KI4"
                            || checkDepo == "KJ4"
                            || checkDepo == "KM7"
                            || checkDepo == "KN6"
                            || checkDepo == "KU3"
                            || checkDepo == "KU8"
                            || checkDepo == "LI5"
                            || checkDepo == "MK5"
                            || checkDepo == "MU5"
                            || checkDepo == "MV7"
                            || checkDepo == "NC8"
                            || checkDepo == "NH2"
                            || checkDepo == "NV6"
                            || checkDepo == "NZ8"
                            || checkDepo == "OM4"
                            || checkDepo == "OR7"
                            || checkDepo == "OR8"
                            || checkDepo == "PK7"
                            || checkDepo == "PK9"
                            || checkDepo == "PS9"
                            || checkDepo == "PV3"
                            || checkDepo == "PZ6"
                            || checkDepo == "RC6"
                            || checkDepo == "RT4"
                            || checkDepo == "RT6"
                            || checkDepo == "RY2"
                            || checkDepo == "SH5"
                            || checkDepo == "SH6"
                            || checkDepo == "SK9"
                            || checkDepo == "SM2"
                            || checkDepo == "SP5"
                            || checkDepo == "SQ4"
                            || checkDepo == "SR7"
                            || checkDepo == "SU8"
                            || checkDepo == "SY5"
                            || checkDepo == "TB3"
                            || checkDepo == "TO8"
                            || checkDepo == "TU3"
                            || checkDepo == "TV6"
                            || checkDepo == "UF5"
                            || checkDepo == "UK4"
                            || checkDepo == "UL9"
                            || checkDepo == "UU3"
                            || checkDepo == "UV4"
                            || checkDepo == "VL4"
                            || checkDepo == "VL5"
                            || checkDepo == "VN4"
                            || checkDepo == "VO4"
                            || checkDepo == "VO6"
                            || checkDepo == "VO8"
                            || checkDepo == "VY6"
                            || checkDepo == "VY8"
                            || checkDepo == "XS7"
                            || checkDepo == "ZP8"
                            
                            )
                        {
                            host.Send(i.ToString());
                            host.Send("<ENTER>");
                            break;
                        }
                        i++;
                    } while (checkDepo.Trim() != "");
                    host.Send("1");
                    host.Send("<ENTER>");
                    Thread.Sleep(2000);

                }
                ForAwait(15, 2, "Consignment Status Entry");
                DBContext.ChangeRecordStatus(id);
            }
            teemApp.Close();
        }

        static bool ForAwait(short col, short row, string keyword)
        {
            byte count = 0;
            
                do
                {
                    count++;
                    
                    if (count > 70)
                    {
                        teemApp.CurrentSession.Network.Close();
                        Thread.Sleep(1000);
                        teemApp.Close();

                        System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("teem2k");

                        foreach (System.Diagnostics.Process p in process)
                        {
                            if (!string.IsNullOrEmpty(p.ProcessName))
                            {
                                try
                                {
                                    p.Kill();
                                }
                                catch
                                { }
                            }
                        }

                        return false;
                    }

                    Thread.Sleep(100);

                } while ((teemApp.CurrentSession.Display.ScreenData[col, row, (short)keyword.Length] != keyword));
            return true;
        }

        static bool ForAwaitRow(short keyword)
        {
            byte count = 0;

            do
            {
                count++;

                if (count > 70)
                {
                    teemApp.CurrentSession.Network.Close();
                    Thread.Sleep(1000);
                    teemApp.Close();

                    System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("teem2k");

                    foreach (System.Diagnostics.Process p in process)
                    {
                        if (!string.IsNullOrEmpty(p.ProcessName))
                        {
                            try
                            {
                                p.Kill();
                            }
                            catch
                            { }
                        }
                    }

                    return false;
                }

                Thread.Sleep(100);

            } while ((teemApp.CurrentSession.Display.CursorRow != keyword));
            return true;
        }
        static bool ForAwaitCol(short keyword)
        {
            byte count = 0;

            do
            {
                count++;

                if (count > 70)
                {
                    teemApp.CurrentSession.Network.Close();
                    Thread.Sleep(1000);
                    teemApp.Close();

                    System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("teem2k");

                    foreach (System.Diagnostics.Process p in process)
                    {
                        if (!string.IsNullOrEmpty(p.ProcessName))
                        {
                            try
                            {
                                p.Kill();
                            }
                            catch
                            { }
                        }
                    }

                    return false;
                }

                Thread.Sleep(100);

            } while ((teemApp.CurrentSession.Display.CursorCol != keyword));
            return true;
        }
    }
}
