using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    using System;
    using System.IO.Ports;
    using System.Threading;

    public class Program
    {
        static bool flag;
        static SerialPort serialPort;

        public static void Main()
        {
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            //传输信息
            string message;

            // 创建串口对象
            serialPort = new SerialPort();

            // 设置端口对象的属性
            serialPort.PortName = SetPortName(serialPort.PortName);//设置端口
            serialPort.BaudRate = SetPortBaudRate(serialPort.BaudRate);//设置波特率
            serialPort.Parity = SetPortParity(serialPort.Parity);//设置端口奇偶值
            serialPort.DataBits = SetPortDataBits(serialPort.DataBits);//设置数据位
            serialPort.StopBits = SetPortStopBits(serialPort.StopBits);//设置停止位
            serialPort.Handshake = SetPortHandshake(serialPort.Handshake);//设置握手协议，直接设置为默认值

            //设置超时时间
            serialPort.ReadTimeout = 550;
            serialPort.WriteTimeout = 550;

            serialPort.Open();//打开一个新的串行端口连接
            flag = true;
            readThread.Start();

            Console.WriteLine();
            Console.WriteLine("连接成功！请输入发送信息。退出请输入“quit”");

            while (flag)
            {
                message = Console.ReadLine();

                //判断退出
                if (stringComparer.Equals("quit", message))
                {
                    flag = false;//flag=0
                }
                else
                {   //发送信息
                    System.DateTime currentTime = new System.DateTime();
                    currentTime = System.DateTime.Now;
                    string strTime = currentTime.ToString();
                    message = "[SENT " + strTime + "] " + message;//发送信息
                    Console.WriteLine(message);//在控制台输出发送的信息
                    serialPort.WriteLine(
                        String.Format("{0}", message));
                }
            }

            readThread.Join();
            serialPort.Close();
        }

        //接收消息
        public static void Read()
        {
            while (flag)//输入quit时flag为0
            {
                try
                {
                    System.DateTime currentTime = new System.DateTime();
                    currentTime = System.DateTime.Now;
                    string strTime = currentTime.ToString();
                    string message = serialPort.ReadLine();
                    message = "[RECV " + strTime + "] " + message;//接收串口信息
                    Console.WriteLine(message);//在控制台输出接收的信息
                }
                catch (TimeoutException) { }
            }
        }

        //设置端口
        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("可用的端口:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.Write("   {0}", s);
            }

            Console.WriteLine();
            Console.Write("请输入，输出的 COM 端口 (默认: {0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {//如果
                portName = defaultPortName;
            }
            return portName;
        }

        // 设置波特率
        public static int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            Console.WriteLine();
            Console.Write("设置波特率(默认:{0}): ", defaultPortBaudRate);
            baudRate = Console.ReadLine();

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        // 设置端口奇偶值
        public static Parity SetPortParity(Parity defaultPortParity)
        {
            string portParity;

            Console.WriteLine();
            Console.WriteLine("可用的奇偶值:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.Write("   {0}", s);
            }

            Console.WriteLine();
            Console.Write("设置奇偶值(默认: {0}):", defaultPortParity.ToString(), true);
            portParity = Console.ReadLine();

            if (portParity == "")
            {
                portParity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), portParity, true);
        }

        //设置数据位
        public static int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits;

            Console.WriteLine();
            Console.Write("设置数据位 (默认: {0}): ", defaultPortDataBits);
            dataBits = Console.ReadLine();

            if (dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits.ToUpperInvariant());
        }

        //设置停止位
        public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits;

            Console.WriteLine();
            Console.WriteLine("可用的停止位:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.Write("   {0}", s);
            }

            Console.WriteLine();
            Console.Write("设置停止位 (默认: {0}):", defaultPortStopBits.ToString());
            stopBits = Console.ReadLine();

            if (stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
        }

        //设置握手协议
        public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            string handshake;
            Console.WriteLine();
            Console.WriteLine("设置默认握手协议：{0}", defaultPortHandshake.ToString());
            handshake = defaultPortHandshake.ToString();
            return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
        }
    }
}