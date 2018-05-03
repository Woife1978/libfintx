﻿/*	
 * 	
 *  This file is part of libfintx.
 *  
 *  Copyright (c) 2016 - 2017 Torsten Klinger
 * 	E-Mail: torsten.klinger@googlemail.com
 * 	
 * 	libfintx is free software; you can redistribute it and/or
 *	modify it under the terms of the GNU Lesser General Public
 * 	License as published by the Free Software Foundation; either
 * 	version 2.1 of the License, or (at your option) any later version.
 *	
 * 	libfintx is distributed in the hope that it will be useful,
 * 	but WITHOUT ANY WARRANTY; without even the implied warranty of
 * 	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * 	Lesser General Public License for more details.
 *	
 * 	You should have received a copy of the GNU Lesser General Public
 * 	License along with libfintx; if not, write to the Free Software
 * 	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
 * 	
 */

//#define WINDOWS

using libfintx.Data;
using System;
using System.IO;

using HBCI = libfintx.Main;

using hbci = libfintx;

#if (DEBUG && WINDOWS)
using System.Windows.Forms;
#endif

namespace libfintx
{
    public class Test
    {
        static bool Anonymous;

        public static void Test_Balance()
        {
            var connectionDetails = new ConnectionDetails()
            {
                Account = "xxx",
                Blz = 76061482,
                BIC = "GENODEF1HSB",
                IBAN = "xxx",
                Url = "https://hbci11.fiducia.de/cgi-bin/hbciservlet",
                HBCIVersion = 300,
                UserId = "xxx",
                Pin = "xxx"
            };
            Anonymous = false;

#region Sync

            /* Sync */

            libfintx.Main.Assembly("libfintx", "0.1");

            libfintx.Main.Tracing(true);

            if (libfintx.Main.Synchronization(connectionDetails, Anonymous))
            {
                Console.WriteLine("[ Sync ]");
                Console.WriteLine();
                Console.WriteLine("Sync ok");
                Console.WriteLine();
            }

            else
            {
                Console.WriteLine("[ Sync ]");
                Console.WriteLine();
                Console.WriteLine(libfintx.Main.Transaction_Output());
                Console.WriteLine();
            }

#endregion

#region balance

            /* Balance */

            var balance = libfintx.Main.Balance(connectionDetails, false);

            Console.WriteLine("[ Balance ]");
            Console.WriteLine();
            Console.WriteLine(balance.Balance);
            Console.WriteLine();

#endregion

            Console.ReadLine();
        }

        public static void Test_Request_TANMediumName()
        {
            var connectionDetails = new ConnectionDetails()
            {
                Blz = 76050101,
                Url = "https://banking-by1.s-fints-pt-by.de/fints30",
                HBCIVersion = 300,
                UserId = "xxx",
                Pin = "xxx"
            };

            bool Anonymous = false;

#region Sync

            /* Sync */

            libfintx.Main.Assembly("libfintx", "0.1");

            libfintx.Main.Tracing(true);

            if (libfintx.Main.Synchronization(connectionDetails, Anonymous))
            {
                Console.WriteLine("[ Sync ]");
                Console.WriteLine();
                Console.WriteLine("Sync ok");
                Console.WriteLine();
            }

            else
            {
                Console.WriteLine("[ Sync ]");
                Console.WriteLine();
                Console.WriteLine(libfintx.Main.Transaction_Output());
                Console.WriteLine();
            }

#endregion

#region tanmediumname

            /* TANMediumname */

            var tanmediumname = libfintx.Main.RequestTANMediumName(connectionDetails);

            Console.WriteLine("[ TAN Medium Name ]");
            Console.WriteLine();
            Console.WriteLine(tanmediumname);
            Console.WriteLine();

#endregion

            Console.ReadLine();
        }

        public static void Test_PhotoTAN()
        {
            var PhotoCode = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\..\\..\\assets\\matrixcode.txt");

            var mCode = new MatrixCode(PhotoCode);

            mCode.CodeImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "matrixcode.png"));
        }

        public static void Test_PushTAN()
        {
            string receiver = string.Empty;
            string receiverIBAN = string.Empty;
            string receiverBIC = string.Empty;
            decimal amount = 0;
            string usage = string.Empty;

            bool anonymous = false;

            ConnectionDetails connectionDetails = new ConnectionDetails()
            {
                AccountHolder = "Torsten Klinger",
                Blz = 76050101,
                BIC = "SSKNDE77XXX",
                IBAN = "xxx",
                Url = "https://banking-by1.s-fints-pt-by.de/fints30",
                HBCIVersion = 300,
                UserId = "xxx",
                Pin = "xxx"
            };

            receiver = "Klinger";
            receiverIBAN = "xxx";
            receiverBIC = "GENODEF1HSB";
            amount = 1.0m;
            usage = "TEST";

            HBCI.Assembly("libfintx", "1");

            HBCI.Tracing(true);

            if (HBCI.Synchronization(connectionDetails, anonymous))
            {
                Segment.HIRMS = "921"; // -> pushTAN

                var tanmediumname = libfintx.Main.RequestTANMediumName(connectionDetails);
                Segment.HITAB = tanmediumname;

                System.Threading.Thread.Sleep(5000);

                Console.WriteLine(HBCI.Transfer(connectionDetails, receiver, receiverIBAN, receiverBIC,
                    amount, usage, Segment.HIRMS, null, anonymous));

                Console.WriteLine(Segment.HITANS);
            }

            var timer = new System.Threading.Timer(
                e => HBCI.Transaction_Output(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            Console.ReadLine();
        }

#if (DEBUG && WINDOWS)
        static bool anonymous = false;

        static string receiver = string.Empty;
        static string receiverIBAN = string.Empty;
        static string receiverBIC = string.Empty;
        static decimal amount = 0;
        static string usage = string.Empty;
        public static ConnectionDetails connectionDetails;

        public static PictureBox pictureBox { get; set; }

        public static void Test_Flicker()
        {
            connectionDetails = new ConnectionDetails()
            {
                AccountHolder = "Torsten Klinger",
                Blz = 76061482,
                BIC = "GENODEF1HSB",
                IBAN = "xxx",
                Url = "https://hbci11.fiducia.de/cgi-bin/hbciservlet",
                HBCIVersion = 300,
                UserId = "xxx",
                Pin = "xxx"
            };

            receiver = "Klinger";
            receiverIBAN = "xxx";
            receiverBIC = "SSKNDE77XXX";
            amount = 1.0m;
            usage = "TEST";

            HBCI.Assembly("libfintx", "1");

            HBCI.Tracing(true);

            if (HBCI.Synchronization(connectionDetails, anonymous))
            {
                Segment.HIRMS = "972"; // -> chip-TAN               

                Image flickerImage = null;
                Console.WriteLine(EncodingHelper.ConvertToUTF8(HBCI.Transfer(connectionDetails, receiver, receiverIBAN, receiverBIC, amount, usage, Segment.HIRMS, anonymous, out flickerImage, 220, 160)));

                Form frm = new Form();
                frm.Size = new Size(flickerImage.Width + 5, flickerImage.Height + 5);
                PictureBox pb = new PictureBox();
                pb.Dock = DockStyle.Fill;
                frm.Controls.Add(pb);
                pb.Image = flickerImage;
                Application.Run(frm);
            }

            var timer = new System.Threading.Timer(
                e => Output(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(10));

            Console.ReadLine();
        }

        static void Output()
        {
            Console.WriteLine(HBCI.Transaction_Output());
        }
#endif

#if (DEBUG && WINDOWS)
        public static void Test_RDH()
        {
            string FilePath = @"D:\rdh.rdh";
            string Pwd = "rdh";

            int BLZ = 0;
            string Url = string.Empty;
            int Port = 0;
            int HBCIVersion = 0;
            string UserID = string.Empty;

            int Country = 280;
            int ProfileVersion = 0;

            BLZ = 76061482;
            Url = "hbci01.fiducia.de";
            Port = 3000;
            HBCIVersion = 300;
            UserID = "xxxx";

            HBCI.Assembly("libfintx", "1.0");

            HBCI.Tracing(true);

            HBCI.Debugging(true);

            // create rdh-10 key file
            hbci.RDHKEY.Create(FilePath, Pwd, BLZ, UserID, Country, ProfileVersion);

            // hbci key
            hbci.RDHKEY.RDHKEYFILE = FilePath;
            hbci.RDHKEY.RDHKEYFILEPWD = Pwd;

            if (!HBCI.Synchronization_RDH(BLZ, Url, Port, HBCIVersion, UserID, FilePath, Pwd))
            {
                Console.WriteLine(HBCI.Transaction_Output());
            }

            Console.ReadLine();
        }
#endif
    }
}