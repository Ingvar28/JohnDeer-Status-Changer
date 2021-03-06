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
using System.Diagnostics;
using System.Data.SqlClient;
using NLog;

namespace Status_changer
{
    //private static Logger logger = LogManager.GetCurrentClassLogger();

    public partial class Form1 : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Form1()
        {

            InitializeComponent();
                                            
        
            int A = 1;

            do
            {

                SqlConnection Сon = null;
                DataTable dt = null;
                SqlDataAdapter da = null;

                // Открытые подключение к базе данных
                string ConnectionString = "server=10.206.64.75;uid=bpa_ru;" +
                    "pwd=bpAut0mat10n_RUbpAut0mat10n_RU; database=BPA_RU";
                Сon = new SqlConnection(ConnectionString);
                Сon.Open();

                // DataSet сохраняет данные в памяти с помощью таблиц данных DataTable
                DataSet dataSet1 = new DataSet();

                // Объект DataAdapter является посредником при взаимодействии базы данных и объекта DataSet
                string select = String.Format("SELECT COUNT(*) FROM dbo.BPA_RU WHERE InMainframe ='0'");
                da = new SqlDataAdapter(select, ConnectionString);

                da = new SqlDataAdapter(select, ConnectionString);
                dt = new DataTable();
                da.Fill(dt);
                string R = dt.Rows[0].ItemArray[0].ToString();
                int pr = 0;
                pr = Convert.ToInt32(R);

                Сon.Close();


                if (pr == 0)
                    Thread.Sleep(5000);

                else
                {

                    try
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
                        host.Send("<ENTER>");
                        host.Send("SM");
                        host.Send("<ENTER>");

                        ForAwait(13, 23, "USER ID");
                        Thread.Sleep(2000);
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
                        Thread.Sleep(500);
                        if (disp.CursorRow != 2)

                        host.Send("YL30");
                        logger.Debug("YL30", this.Text); //LOG
                        host.Send("<ENTER>");

                        foreach (DataRow row in consData.Rows)
                        {
                            //ДОПИСАТЬ ЗАБОР ПЕРЕМЕННЫХ ИЗ БАЗЫ1!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            var qty = "1";
                            //int id = Convert.ToInt32(row["id"].ToString());
                            var con = row["Nakladnaya TNT"].ToString();
                            DateTime dateFromBase = row.Field<DateTime>("Date");
                            var date = dateFromBase.ToString("ddMMMyy", CultureInfo.GetCultureInfo("en-us"));
                            var time = dateFromBase.ToString("HHmm");
                            var status = row["Status"].ToString();
                            //var comment = row["Commentary"].ToString();
                            var eventdepot = row["EventDepot"].ToString();  //дописал
                            //if (comment.Trim() == "") comment = "...";
                            var delvz = "B";

                            //Начало подключения к MF
                            ForAwait(15, 2, "Consignment Status Entry");
                            Thread.Sleep(500);
                            host.Send(status);//Вводим статус
                            logger.Debug(status, this.Text); //LOG
                            Thread.Sleep(500); //костыль
                            if (disp.CursorCol != 28 && disp.CursorCol != 10) host.Send("<TAB>");
                            ForAwaitCol(28);

                            host.Send(date);
                            logger.Debug(date, this.Text);  //LOG
                            host.Send("<TAB>");
                            ForAwaitCol(46);

                            host.Send(time);
                            if (disp.CursorCol != 70 && disp.CursorCol != 46) host.Send("<TAB>");
                            ForAwaitCol(70);

                            Thread.Sleep(500);
                            if (eventdepot == "Москва")
                                {
                                    eventdepot = "MS1";
                                    host.Send(eventdepot);
                                }

                            else if ( eventdepot== "Владимир"
                            #region VL5_depot    
                                || eventdepot == "Ковров-1"
                                || eventdepot == "Вязники"
                                || eventdepot == "Гороховец"
                                || eventdepot == "Дзержинск"                                                                               
                            #endregion
                                    )
                                {
                                    eventdepot = "VL5";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Н.Новгород Моск."// Уточнение станции!!!!
                            #region GOJ_depot
                                || eventdepot == "Сухобезводно"
                                #endregion
                                    )
                                {
                                    eventdepot = "GOJ";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Киров"
                            #region KI4_depot    
                                || eventdepot == "Глазов"
                                || eventdepot == "Балезино"
                                || eventdepot == "Верещагино"
                            #endregion
                                    )
                                {
                                    eventdepot = "KI4";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Пермь"
                            #region RT4_depot    
                                || eventdepot == "Ферма"
                                || eventdepot == "Кунгур"
                            #endregion
                                    )
                                {
                                    eventdepot = "RT4";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Екатеринбург-П"// Уточнение станции!!!!
                            #region SVX_depot    
                                || eventdepot == "Аксариха"
                            #endregion
                                    )
                                {
                                    eventdepot = "SVX";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Тюмень"
                            #region RT6_depot    
                                || eventdepot == "Заводоуковская"
                                || eventdepot == "Омутинская"
                                || eventdepot == "Ишим"
                                || eventdepot == "Называевская"
                            #endregion
                                    )
                                {
                                    eventdepot = "RT6";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Омск-пасс")// Уточнение станции!!!!
                            {
                                    eventdepot = "OM4";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Барабинск")
                                {
                                    eventdepot = "BB8";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Новосибирск-гл."// Уточнение станции!!!!
                            #region OVB_depot    
                                || eventdepot == "Тайга"
                                || eventdepot == "Мариинск"
                                || eventdepot == "Боготол"
                                || eventdepot == "Ачинск 1"
                                || eventdepot == "Козулька"
                            #endregion
                                    )
                                {
                                    eventdepot = "OVB";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Красноярск-пасс."// Уточнение станции!!!!
                            #region KJ4_depot    
                                || eventdepot == "Базаиха"
                                || eventdepot == "Уяр"
                                || eventdepot == "Заозерная"
                                || eventdepot == "Канск-Енисейский"
                                || eventdepot == "Иланская"
                                || eventdepot == "Решоты"
                                || eventdepot == "Тайшет"
                                || eventdepot == "Нижнеудинск"
                                || eventdepot == "Тулун"
                                || eventdepot == "Зима"
                                || eventdepot == "Черемхово"
                                || eventdepot == "Усолье-Сибирское"
                                || eventdepot == "Ангарск"
                            #endregion
                                    )
                                {
                                    eventdepot = "KJ4";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Иркутск-пасс."// Уточнение станции!!!!
                            #region IK3_depot    
                                || eventdepot == "Слюдянка"
                                || eventdepot == "Мысовая"
                            #endregion
                                    )
                                {
                                    eventdepot = "IK3";
                                    host.Send(eventdepot);
                            }

                            else if (eventdepot == "Улан-Удэ"
                            #region UU3_depot    
                                || eventdepot == "Петр.Завод Чет"// Уточнение станции!!!!
                                || eventdepot == "Хилок"
                                || eventdepot == "Могзон"
                                || eventdepot == "Кадала"
                            #endregion
                                    )
                            {
                                eventdepot = "UU3";
                                host.Send(eventdepot);
                            }

                            else if (eventdepot == "Чита 2"// Уточнение станции!!!!
                            #region CT6_depot    
                                || eventdepot == "Карымская"
                                || eventdepot == "Шилка пасс."// Уточнение станции!!!!
                                || eventdepot == "Приисковая"
                                || eventdepot == "Куэнга"
                                || eventdepot == "Чернышевск Заб."// Уточнение станции!!!!
                                || eventdepot == "Зилово"
                                || eventdepot == "Ксеньевская"
                                || eventdepot == "Могоча"
                                || eventdepot == "Ерофей Павлович"
                                || eventdepot == "Уруша"
                                || eventdepot == "Сковородино"
                                || eventdepot == "Магдагачи"
                                || eventdepot == "Тыгда"
                                || eventdepot == "Шимановская"
                                || eventdepot == "Свободный"
                                || eventdepot == "Белогорск"
                                || eventdepot == "Завитая"
                                || eventdepot == "Бурея"
                                || eventdepot == "Облучье"
                                || eventdepot == "Биробиджан"
                            #endregion
                                    )
                                {
                                    eventdepot = "CT6";
                                    host.Send(eventdepot);
                                }

                            else if (eventdepot == "Хабаровск 1"// Уточнение станции!!!!
                            #region KH6_depot    
                                || eventdepot == "Вяземская"
                                || eventdepot == "Бикин"
                                || eventdepot == "Лучегорск"
                                || eventdepot == "Дальнереченск 1"
                                || eventdepot == "Ружино"
                                || eventdepot == "Спасск-Дальний"
                                || eventdepot == "Мучная"
                                || eventdepot == "Уссурийск"
                            #endregion
                                    )
                                {
                                    eventdepot = "KH6";
                                    host.Send(eventdepot);
                                }
                            else if (eventdepot == "Владивосток")
                            {
                                eventdepot = "VK3";
                                host.Send(eventdepot);
                            }

                            Thread.Sleep(500);
                            logger.Debug(eventdepot, this.Text); //LOG
                            host.Send("<TAB>");
                            ForAwaitCol(13);
                            host.Send("<TAB>");
                            ForAwaitCol(57);

                            //host.Send(date);
                            host.Send("<TAB>");
                            ForAwaitCol(77);

                            ForAwaitCol(77);//Rems + Если статус OF, то делаем и вводим коммент = статусу OF
                            if (status == "OF")
                            {
                                host.Send("<F4>");
                                ForAwait(5, 5, "Seq Remarks");
                                host.Send(status);
                                Thread.Sleep(500);
                                host.Send("<ENTER>");

                                ForAwaitCol(9); // вторая строка seq remarks
                                host.Send("<F12>");

                                ForAwaitCol(18);// mode: add - пропускаем
                                host.Send("<F12>");//возвращаемся в общее меню на позицию REMS+ COL(77)
                                ForAwait(15, 2, "Consignment Status Entry");// проверяем                    
                            }
                            host.Send("<TAB>");

                            host.Send("<ENTER>");
                            ForAwaitCol(9);
                            host.Send("<F12>");
                            ForAwaitCol(18);
                            host.Send("<F12>");
                            ForAwait(15, 2, "Consignment Status Entry");

                            host.Send("<TAB>");

                            ForAwaitCol(12);//Runsheet - пропускаем
                            host.Send("<TAB>");

                            ForAwaitCol(33);//Round no - пропускаем
                            host.Send("<TAB>");

                            ForAwaitCol(54);// Delv zone -  по умолчанию "b"
                            host.Send(delvz);
                            host.Send("<TAB>");

                            ForAwaitCol(73);// Delv area - пропускаем
                            host.Send("<TAB>");

                            ForAwaitCol(24);//No of status Entries = 1
                            host.Send(qty);
                            host.Send("<ENTER>");
                            ForAwait(1, 10, "01");

                            host.Send(con);  // Con number        
                            logger.Debug(con, this.Text); //LOG
                            ForAwaitCol(26);//Позиция после ввода 9 символов номера накладной    
                            host.Send("<TAB>");

                            ForAwaitCol(37);// Статус (повторный вывод) - пропускаем
                            host.Send("<TAB>");

                            ForAwaitCol(48);// Time - пропускаем
                            host.Send("<TAB>");

                            ForAwaitCol(58);// Solved - пропускаем
                            host.Send("<TAB>");

                            ForAwaitCol(64);// Rev date (повторный вывод) - пропускаем
                            host.Send("<TAB>");

                            ForAwaitCol(17); // Signatory Если статус OK = OK, если OF = ""
                            if (status == "OK")
                            {
                                host.Send(status);
                            }
                            else
                            {
                                host.Send("");
                            }

                            host.Send("<ENTER>");//концовка и переход обратно к вводу статуса
                            host.Send("<F12>");
                            host.Send("<ENTER>");
                            Thread.Sleep(2500);

                            if (disp.ScreenData[15, 2, 9] == "Duplicate")
                            {
                                var checkDepo = "";
                                short i = 1;
                                do
                                {
                                    short col = (Int16)(9 + i);
                                    checkDepo = disp.ScreenData[54, col, 3];
                                    if (checkDepo == "MW3"
                                    #region depoes    
                            || checkDepo == "MW5"
                                        || checkDepo == "MW7"
                                        || checkDepo == "MOW"
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
                                    #endregion
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
                                Thread.Sleep(2500);

                            }
                            ForAwait(15, 2, "Consignment Status Entry");
                            //DBContext.ChangeRecordStatus(id);
                        }



                        teemApp.Close();
                        foreach (Process proc in Process.GetProcessesByName("teem2k"))
                        {
                            proc.Kill();
                        }
                        teemApp.Application.Close();
                        Thread.Sleep(1000);
                        host.Send("<ENTER>");




                    }
                    catch (Exception ex)
                    {
                        // Вывод сообщения об ошибке
                        logger.Debug(ex.ToString());
                    }
                }
            }
            
            while (A == 1);

        }
        
        static teemtalk. Application teemApp;

        public string EventDepot { get; private set; }
        

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
                            catch (Exception ex)
                            {
                                // Вывод сообщения об ошибке
                                logger.Debug(ex.ToString());
                            }
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
                            catch (Exception ex)
                            {
                                // Вывод сообщения об ошибке
                                logger.Debug(ex.ToString());
                            }
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
                            catch (Exception ex)
                            {
                                // Вывод сообщения об ошибке
                                logger.Debug(ex.ToString());
                            }
                        }
                    }

                    return false;
                }

                Thread.Sleep(100);

            } while ((teemApp.CurrentSession.Display.CursorCol != keyword));
            return true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }
    }
}

