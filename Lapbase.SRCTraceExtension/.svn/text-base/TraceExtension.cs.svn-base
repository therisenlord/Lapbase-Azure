using System;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;


  // Define a SOAP Extension that traces the SOAP request and SOAP
  // response for the XML Web service method the SOAP extension is
  // applied to.
namespace TraceExtension
{
    public class TraceExtension : SoapExtension
    {
        public Stream oldStream;
        public Stream newStream;
        string filename = "C:\\Projects\\Lapbase\\Lapbase.Bold\\trace.log";

        // Save the Stream representing the SOAP request or SOAP response into
        // a local memory buffer.
        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        ///<summary>
        /// When the SOAP extension is accessed for the first time, the XML Web
        /// service method it is applied to is accessed to store the file
        /// name passed in, using the corresponding SoapExtensionAttribute.   
        ///</summary>
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return ((TraceExtensionAttribute)attribute).Filename;
        }

        ///<summary>
        /// The SOAP extension was configured to run using a configuration file
        /// instead of an attribute applied to a specific XML Web service
        /// method.
        ///</summary>
        public override object GetInitializer(Type WebServiceType)
        {
            // Return a file name to log the trace information to, based on the
            // type.
            return WebServiceType.GetType().ToString() + ".log";
        }

        ///<summary>
        /// Receive the file name stored by GetInitializer and store it in a
        /// member variable for this specific instance.
        ///</summary>
        public override void Initialize(object initializer)
        {
            filename = (string)initializer;
        }


        ///<summary>
        ///If the SoapMessageStage is such that the SoapRequest or
        ///SoapResponse is still in the SOAP format to be sent or received,
        ///save it out to a file.
        ///</summary>
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    WriteOutput(message);
                    break;
                case SoapMessageStage.AfterSerialize:
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    //WriteInput(message);
                    break;
                case SoapMessageStage.AfterDeserialize:
                    //WriteInput(message);
                    break;
            }
        }

        public void WriteOutput(SoapMessage message)
        {
            FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fs);

            if (message is SoapClientMessage)
            {
                if (message.MethodInfo.InParameters.Length > 0)
                {
                    object obj = message.GetInParameterValue(0);
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                    xs.Serialize(streamWriter, obj);
                }
            }

            streamWriter.Flush();
            streamWriter.Close();
        }

        //public void WriteInput(SoapMessage message)
        //{
        //    FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write);
        //    StreamWriter streamWriter = new StreamWriter(fs);

        //    if (message is SoapServerMessage)
        //    {
        //        streamWriter.WriteLine("METHOD CALL at " + DateTime.Now);
        //        int ipc = message.MethodInfo.InParameters.Length;
        //        if (ipc > 0) streamWriter.WriteLine("  Parameters:");
        //        for (int n = 0; n < ipc; n++)
        //            streamWriter.WriteLine("     " + message.GetInParameterValue(n));
        //    }
        //    streamWriter.Flush();
        //    streamWriter.Close();
        //}

        // CopyStream copies the contents of a source stream 
        // to a destination stream
        private void CopyStream(Stream src, Stream dest)
        {
            StreamReader reader = new StreamReader(src);
            StreamWriter writer = new StreamWriter(dest);
            writer.Write(reader.ReadToEnd());
            writer.Flush();
        }

    }

    // Create a SoapExtensionAttribute for the SOAP Extension that can be
    // applied to an XML Web service method.
    [AttributeUsage(AttributeTargets.Method)]
    public class TraceExtensionAttribute : SoapExtensionAttribute
    {

        private string filename = "C:\\Projects\\Lapbase\\Lapbase.Bold\\trace.log";
        private int priority;
        string tag;

        public override Type ExtensionType
        {
            get { return typeof(TraceExtension); }
        }

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public string Filename
        {
            get
            {
                return filename;
            }
            set
            {
                filename = value;
            }
        }
    }
}