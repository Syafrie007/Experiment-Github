using CefSharp;
using CefSharp.WinForms;
using OtpNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Experiment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CefSettings cfsettings = new CefSettings();
            cfsettings.UserAgent = "My/Custom/User-Agent-AndStuff";
            Cef.Initialize(cfsettings);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var b=browser.GetBrowser();
            browser.Load("https://www.google.com/");
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
            byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
                    StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        static string GetCharsFromKeys(Keys keys, bool shift, bool altGr)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (shift)
                keyboardState[(int)Keys.ShiftKey] = 0xff;
            if (altGr)
            {
                keyboardState[(int)Keys.ControlKey] = 0xff;
                keyboardState[(int)Keys.Menu] = 0xff;
            }
            ToUnicode((uint)keys, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var pat = "//input[@class='gLFyf gsfi']";
            string script = $@"document.evaluate(`{pat}`, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.focus();";
            string script2 = $@"document.evaluate(`//a[@class='gb_4 gb_5 gb_ae gb_4c']`, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click();";
            string script3 = $@"

var event = new MouseEvent('mouseover', {{
  'view': window,
  'bubbles': true,
  'cancelable': true
}});

document.evaluate(`//a[@class='gb_D']`, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.dispatchEvent(event);";

            browser.ExecuteScriptAsync(script);

            KeyEvent[] events = new KeyEvent[] {
                new KeyEvent() { FocusOnEditableField = true, WindowsKeyCode = GetCharsFromKeys(Keys.R, false, false)[0], Modifiers = CefEventFlags.None, Type = KeyEventType.Char, IsSystemKey = false }, // Just the letter R, no shift (so no caps...?)
                new KeyEvent() { FocusOnEditableField = true, WindowsKeyCode = GetCharsFromKeys(Keys.R, true, false)[0], Modifiers = CefEventFlags.ShiftDown, Type = KeyEventType.Char, IsSystemKey = false }, // Capital R?
                new KeyEvent() { FocusOnEditableField = true, WindowsKeyCode = GetCharsFromKeys(Keys.D4, false, false)[0], Modifiers = CefEventFlags.None, Type = KeyEventType.Char, IsSystemKey = false }, // Just the number 4
                new KeyEvent() { FocusOnEditableField = true, WindowsKeyCode = GetCharsFromKeys(Keys.D4, true, false)[0], Modifiers = CefEventFlags.ShiftDown, Type = KeyEventType.Char, IsSystemKey = false }, // Shift 4 (should be $)
            };
            foreach (KeyEvent ev in events)
            {
                Thread.Sleep(200);
                Application.DoEvents();
                browser.GetBrowser().GetHost().SendKeyEvent(ev);
            }
            Thread.Sleep(200);

            browser.ExecuteScriptAsync(script3);
            //browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(script);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public byte[] encrypt(ProvisionMessage message)// throws InvalidKeyException
        {
            ECKeyPair ourKeyPair = Curve.generateKeyPair();
            byte[] sharedSecret = Curve.calculateAgreement(theirPublicKey, ourKeyPair.getPrivateKey());
            byte[] derivedSecret = new HKDFv3().deriveSecrets(sharedSecret, Encoding.UTF8.GetBytes("TextSecure Provisioning Message"), 64);
            byte[][] parts = Util.split(derivedSecret, 32, 32);

            byte[] version = { 0x01 };
            byte[] ciphertext = getCiphertext(parts[0], message.ToByteArray());
            byte[] mac = getMac(parts[1], Util.join(version, ciphertext));
            byte[] body = Util.join(version, ciphertext, mac);

            return ProvisionEnvelope.CreateBuilder()
                                    .SetPublicKey(ByteString.CopyFrom(ourKeyPair.getPublicKey().serialize()))
                                    .SetBody(ByteString.CopyFrom(body))
                                    .Build()
                                    .ToByteArray();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
