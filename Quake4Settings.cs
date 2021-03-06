﻿using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.Quake4
{
    public partial class Quake4Settings : UserControl
    {
        public bool AutoReset { get; set; }
        public bool AutoStart { get; set; }
        public bool sC01 { get; set; }
        public bool sC02 { get; set; }
        public bool sC03 { get; set; }
        public bool sC04 { get; set; }
        public bool sC05 { get; set; }
        public bool sC06 { get; set; }
        public bool sC07 { get; set; }
        public bool sC08 { get; set; }
        public bool sC09 { get; set; }
        public bool sC10 { get; set; }
        public bool sC11 { get; set; }
        public bool sC12 { get; set; }
        public bool sC13 { get; set; }
        public bool sC14 { get; set; }
        public bool sC15 { get; set; }
        public bool sC16 { get; set; }
        public bool sC17 { get; set; }
        public bool sC18 { get; set; }
        public bool sC19 { get; set; }
        public bool sC20 { get; set; }
        public bool sC21 { get; set; }
        public bool sC22 { get; set; }
        public bool sC23 { get; set; }
        public bool sC24 { get; set; }
        public bool sC25 { get; set; }
        public bool sC26 { get; set; }
        public bool sC27 { get; set; }
        public bool sC28 { get; set; }
        public bool sC29 { get; set; }
        public bool sC30 { get; set; }
        public bool sC31 { get; set; }

        private const bool DEFAULT_AUTORESET = false;
        private const bool DEFAULT_AUTOSTART = true;
        private const bool DEFAULT_C01 = true;
        private const bool DEFAULT_C02 = true;
        private const bool DEFAULT_C03 = true;
        private const bool DEFAULT_C04 = true;
        private const bool DEFAULT_C05 = true;
        private const bool DEFAULT_C06 = true;
        private const bool DEFAULT_C07 = true;
        private const bool DEFAULT_C08 = true;
        private const bool DEFAULT_C09 = true;
        private const bool DEFAULT_C10 = true;
        private const bool DEFAULT_C11 = true;
        private const bool DEFAULT_C12 = true;
        private const bool DEFAULT_C13 = true;
        private const bool DEFAULT_C14 = true;
        private const bool DEFAULT_C15 = true;
        private const bool DEFAULT_C16 = true;
        private const bool DEFAULT_C17 = true;
        private const bool DEFAULT_C18 = true;
        private const bool DEFAULT_C19 = true;
        private const bool DEFAULT_C20 = true;
        private const bool DEFAULT_C21 = true;
        private const bool DEFAULT_C22 = true;
        private const bool DEFAULT_C23 = true;
        private const bool DEFAULT_C24 = true;
        private const bool DEFAULT_C25 = true;
        private const bool DEFAULT_C26 = true;
        private const bool DEFAULT_C27 = true;
        private const bool DEFAULT_C28 = true;
        private const bool DEFAULT_C29 = true;
        private const bool DEFAULT_C30 = true;
        private const bool DEFAULT_C31 = true;



        public Quake4Settings()
        {
            InitializeComponent();

            this.chkAutoReset.DataBindings.Add("Checked", this, "AutoReset", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkAutoStart.DataBindings.Add("Checked", this, "AutoStart", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk01.DataBindings.Add("Checked", this, "sC01", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk02.DataBindings.Add("Checked", this, "sC02", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk03.DataBindings.Add("Checked", this, "sC03", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk04.DataBindings.Add("Checked", this, "sC04", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk05.DataBindings.Add("Checked", this, "sC05", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk06.DataBindings.Add("Checked", this, "sC06", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk07.DataBindings.Add("Checked", this, "sC07", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk08.DataBindings.Add("Checked", this, "sC08", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk09.DataBindings.Add("Checked", this, "sC09", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk10.DataBindings.Add("Checked", this, "sC10", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk11.DataBindings.Add("Checked", this, "sC11", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk12.DataBindings.Add("Checked", this, "sC12", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk13.DataBindings.Add("Checked", this, "sC13", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk14.DataBindings.Add("Checked", this, "sC14", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk15.DataBindings.Add("Checked", this, "sC15", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk16.DataBindings.Add("Checked", this, "sC16", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk17.DataBindings.Add("Checked", this, "sC17", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk18.DataBindings.Add("Checked", this, "sC18", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk19.DataBindings.Add("Checked", this, "sC19", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk20.DataBindings.Add("Checked", this, "sC20", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk21.DataBindings.Add("Checked", this, "sC21", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk22.DataBindings.Add("Checked", this, "sC22", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk23.DataBindings.Add("Checked", this, "sC23", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk24.DataBindings.Add("Checked", this, "sC24", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk25.DataBindings.Add("Checked", this, "sC25", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk26.DataBindings.Add("Checked", this, "sC26", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk27.DataBindings.Add("Checked", this, "sC27", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk28.DataBindings.Add("Checked", this, "sC28", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk29.DataBindings.Add("Checked", this, "sC29", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk30.DataBindings.Add("Checked", this, "sC30", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chk31.DataBindings.Add("Checked", this, "sC31", false, DataSourceUpdateMode.OnPropertyChanged);

            // defaults
            this.AutoReset = DEFAULT_AUTORESET;
            this.AutoStart = DEFAULT_AUTOSTART;
            this.sC01 = DEFAULT_C01;
            this.sC02 = DEFAULT_C02;
            this.sC03 = DEFAULT_C03;
            this.sC04 = DEFAULT_C04;
            this.sC05 = DEFAULT_C05;
            this.sC06 = DEFAULT_C06;
            this.sC07 = DEFAULT_C07;
            this.sC08 = DEFAULT_C08;
            this.sC09 = DEFAULT_C09;
            this.sC10 = DEFAULT_C10;
            this.sC11 = DEFAULT_C11;
            this.sC12 = DEFAULT_C12;
            this.sC13 = DEFAULT_C13;
            this.sC14 = DEFAULT_C14;
            this.sC15 = DEFAULT_C15;
            this.sC16 = DEFAULT_C16;
            this.sC17 = DEFAULT_C17;
            this.sC18 = DEFAULT_C18;
            this.sC19 = DEFAULT_C19;
            this.sC20 = DEFAULT_C20;
            this.sC21 = DEFAULT_C21;
            this.sC22 = DEFAULT_C22;
            this.sC23 = DEFAULT_C23;
            this.sC24 = DEFAULT_C24;
            this.sC25 = DEFAULT_C25;
            this.sC26 = DEFAULT_C26;
            this.sC27 = DEFAULT_C27;
            this.sC28 = DEFAULT_C28;
            this.sC29 = DEFAULT_C29;
            this.sC30 = DEFAULT_C30;
            this.sC31 = DEFAULT_C31;

        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");

            settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

            settingsNode.AppendChild(ToElement(doc, "AutoReset", this.AutoReset));
            settingsNode.AppendChild(ToElement(doc, "AutoStart", this.AutoStart));
            settingsNode.AppendChild(ToElement(doc, "C1", this.sC01));
            settingsNode.AppendChild(ToElement(doc, "C2", this.sC02));
            settingsNode.AppendChild(ToElement(doc, "C3", this.sC03));
            settingsNode.AppendChild(ToElement(doc, "C4", this.sC04));
            settingsNode.AppendChild(ToElement(doc, "C5", this.sC05));
            settingsNode.AppendChild(ToElement(doc, "C6", this.sC06));
            settingsNode.AppendChild(ToElement(doc, "C7", this.sC07));
            settingsNode.AppendChild(ToElement(doc, "C8", this.sC08));
            settingsNode.AppendChild(ToElement(doc, "C9", this.sC09));
            settingsNode.AppendChild(ToElement(doc, "C10", this.sC10));
            settingsNode.AppendChild(ToElement(doc, "C10", this.sC10));
            settingsNode.AppendChild(ToElement(doc, "C11", this.sC11));
            settingsNode.AppendChild(ToElement(doc, "C12", this.sC12));
            settingsNode.AppendChild(ToElement(doc, "C13", this.sC13));
            settingsNode.AppendChild(ToElement(doc, "C14", this.sC14));
            settingsNode.AppendChild(ToElement(doc, "C15", this.sC15));
            settingsNode.AppendChild(ToElement(doc, "C16", this.sC16));
            settingsNode.AppendChild(ToElement(doc, "C17", this.sC17));
            settingsNode.AppendChild(ToElement(doc, "C18", this.sC18));
            settingsNode.AppendChild(ToElement(doc, "C19", this.sC19));
            settingsNode.AppendChild(ToElement(doc, "C20", this.chk20));
            settingsNode.AppendChild(ToElement(doc, "C21", this.chk21));
            settingsNode.AppendChild(ToElement(doc, "C22", this.chk22));
            settingsNode.AppendChild(ToElement(doc, "C23", this.chk23));
            settingsNode.AppendChild(ToElement(doc, "C24", this.chk24));
            settingsNode.AppendChild(ToElement(doc, "C25", this.chk25));
            settingsNode.AppendChild(ToElement(doc, "C26", this.chk26));
            settingsNode.AppendChild(ToElement(doc, "C27", this.chk27));
            settingsNode.AppendChild(ToElement(doc, "C28", this.chk28));
            settingsNode.AppendChild(ToElement(doc, "C29", this.chk29));
            settingsNode.AppendChild(ToElement(doc, "C30", this.chk30));
            settingsNode.AppendChild(ToElement(doc, "C31", this.chk31));

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            this.AutoReset = ParseBool(settings, "AutoReset", DEFAULT_AUTORESET);
            this.AutoStart = ParseBool(settings, "AutoStart", DEFAULT_AUTOSTART);
            this.sC01 = ParseBool(settings, "C1", DEFAULT_C01);
            this.sC02 = ParseBool(settings, "C2", DEFAULT_C02);
            this.sC03 = ParseBool(settings, "C3", DEFAULT_C03);
            this.sC04 = ParseBool(settings, "C4", DEFAULT_C04);
            this.sC05 = ParseBool(settings, "C5", DEFAULT_C05);
            this.sC06 = ParseBool(settings, "C6", DEFAULT_C06);
            this.sC07 = ParseBool(settings, "C7", DEFAULT_C07);
            this.sC08 = ParseBool(settings, "C8", DEFAULT_C08);
            this.sC09 = ParseBool(settings, "C9", DEFAULT_C09);
            this.sC10 = ParseBool(settings, "C10", DEFAULT_C10);
            this.sC11 = ParseBool(settings, "C11", DEFAULT_C11);
            this.sC12 = ParseBool(settings, "C12", DEFAULT_C12);
            this.sC13 = ParseBool(settings, "C13", DEFAULT_C13);
            this.sC14 = ParseBool(settings, "C14", DEFAULT_C14);
            this.sC15 = ParseBool(settings, "C15", DEFAULT_C15);
            this.sC16 = ParseBool(settings, "C16", DEFAULT_C16);
            this.sC17 = ParseBool(settings, "C17", DEFAULT_C17);
            this.sC18 = ParseBool(settings, "C18", DEFAULT_C18);
            this.sC19 = ParseBool(settings, "C19", DEFAULT_C19);
            this.sC20 = ParseBool(settings, "C20", DEFAULT_C20);
            this.sC21 = ParseBool(settings, "C21", DEFAULT_C21);
            this.sC22 = ParseBool(settings, "C22", DEFAULT_C22);
            this.sC23 = ParseBool(settings, "C23", DEFAULT_C23);
            this.sC24 = ParseBool(settings, "C24", DEFAULT_C24);
            this.sC25 = ParseBool(settings, "C25", DEFAULT_C25);
            this.sC26 = ParseBool(settings, "C26", DEFAULT_C26);
            this.sC27 = ParseBool(settings, "C27", DEFAULT_C27);
            this.sC28 = ParseBool(settings, "C28", DEFAULT_C28);
            this.sC29 = ParseBool(settings, "C29", DEFAULT_C29);
            this.sC30 = ParseBool(settings, "C30", DEFAULT_C30);
            this.sC31 = ParseBool(settings, "C31", DEFAULT_C31);
        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            bool val;
            return settings[setting] != null ?
                (Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_)
                : default_;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }

        private void Quake4Settings_Load(object sender, EventArgs e)
        {

        }
    }
}
