using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessStartListners
{
    public partial class Form1 : Form
    {
        string ComputerName = "localhost";
        string WmiQuery;
        ManagementEventWatcher Watcher;
        ManagementScope Scope;
        ManagementClass objMC;
        ManagementObjectCollection objMOC;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text += "Listening process creation, Press Enter to exit";
            try
            {


                Scope = new ManagementScope(String.Format("\\\\{0}\\root\\CIMV2", ComputerName), null);
                Scope.Connect();

                WmiQuery = "Select * From __InstanceCreationEvent Within 1 " +
                "Where TargetInstance ISA 'Win32_Process' ";

                Watcher = new ManagementEventWatcher(Scope, new EventQuery(WmiQuery));
                Watcher.EventArrived += new EventArrivedEventHandler(this.WmiEventHandler);
                Watcher.Start();
                //  Console.Read();

            }
            catch (Exception ex)
            {
                textBox1.Text += string.Format("Exception {0} Trace {1}", ex.Message, ex.StackTrace);
            }
        }

        private void WmiEventHandler(object sender, EventArrivedEventArgs e)
        {
            //in this point the new events arrives
            //you can access to any property of the Win32_Process class

            var obj = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            var data = GetWin32_ProcessDetails(obj);
            this.Invoke(new Action(() =>
            {

                textBox1.Text += "TargetInstance.Handle :    " + ((ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value)["Handle"] + Environment.NewLine;
                textBox1.Text += "TargetInstance.Name :      " + ((ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value)["Name"] + Environment.NewLine;

            }));
        }
        public Win32_Process GetWin32_ProcessDetails(object obj = null)
        {
            //objMC = new ManagementClass("Win32_ComputerSystem");
            //objMOC = objMC.GetInstances();

            Win32_Process objData = new Win32_Process();
            var _type = typeof(Win32_Process);
            var _properties = _type.GetProperties();
            //foreach (ManagementObject item in objMOC)
            //{
            var item = (ManagementBaseObject)obj;
            for (int i = 0; i < _properties.Length; i++)
            {
                try
                {

                    if (item.Properties[_properties[i].Name].Value != null)
                        objData.GetType().GetProperty(_properties[i].Name).SetValue(objData, item.Properties[_properties[i].Name].Value, null);
                }
                catch (Exception)
                {

                }
            }
            //}
            return objData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Watcher.Stop();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }


    }
    public class Win32_Process
    {
        public string CreationClassName { get; set; }
        public string Caption { get; set; }
        public string CommandLine { get; set; }
        public string CreationDate { get; set; }
        public string CSCreationClassName { get; set; }
        public string CSName { get; set; }
        public string Description { get; set; }
        public string ExecutablePath { get; set; }
        public string ExecutionState { get; set; }
        public string Handle { get; set; }
        public string HandleCount { get; set; }
        public string InstallDate { get; set; }
        public string KernelModeTime { get; set; }
        public string MaximumWorkingSetSize { get; set; }
        public string MinimumWorkingSetSize { get; set; }
        public string Name { get; set; }
        public string OSCreationClassName { get; set; }
        public string OSName { get; set; }
        public string OtherOperationCount { get; set; }
        public string OtherTransferCount { get; set; }
        public string PageFaults { get; set; }
        public string PageFileUsage { get; set; }
        public string ParentProcessId { get; set; }
        public string PeakPageFileUsage { get; set; }
        public string PeakVirtualSize { get; set; }
        public string PeakWorkingSetSize { get; set; }
        public string Priority { get; set; }
        public string PrivatePageCount { get; set; }
        public string ProcessId { get; set; }
        public string QuotaNonPagedPoolUsage { get; set; }
        public string QuotaPagedPoolUsage { get; set; }
        public string QuotaPeakNonPagedPoolUsage { get; set; }
        public string QuotaPeakPagedPoolUsage { get; set; }
        public string ReadOperationCount { get; set; }
        public string ReadTransferCount { get; set; }
        public string SessionId { get; set; }
        public string Status { get; set; }
        public string TerminationDate { get; set; }
        public string ThreadCount { get; set; }
        public string UserModeTime { get; set; }
        public string VirtualSize { get; set; }
        public string WindowsVersion { get; set; }
        public string WorkingSetSize { get; set; }
        public string WriteOperationCount { get; set; }
        public string WriteTransferCount { get; set; }
    }
}
