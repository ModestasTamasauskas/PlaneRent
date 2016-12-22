using System.Security.Principal;
using System.Threading;
using System.Timers;
using System.Transactions;
using Core.Common.Core;
using PlaneRental.Business.Bootstrapper;
using PlaneRental.Business.Entities;
using PlaneRental.Business.Managers;
using SM = System.ServiceModel;

namespace PlaneRental.ServiceHost.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericPrincipal principal = new GenericPrincipal(
                new GenericIdentity("Miguel"), new string[] { "Administrators", "PlaneRentalAdmin" });
            Thread.CurrentPrincipal = principal;

            ObjectBase.Container = MefLoader.Init();

            System.Console.WriteLine("Starting up services...");
            System.Console.WriteLine("");

            SM.ServiceHost hostInventoryManager = new SM.ServiceHost(typeof(InventoryManager));
            SM.ServiceHost hostRentalManager = new SM.ServiceHost(typeof(RentalManager));
            SM.ServiceHost hostAccountManager = new SM.ServiceHost(typeof(AccountManager));

            StartService(hostInventoryManager, "InventoryManager");
            StartService(hostRentalManager, "RentalManager");
            StartService(hostAccountManager, "AccountManager");

            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();

            System.Console.WriteLine("Reservation monitor started.");

            System.Console.WriteLine("");
            System.Console.WriteLine("Press [Enter] to exit.");
            System.Console.ReadLine();
            System.Console.WriteLine("");

            timer.Stop();

            System.Console.WriteLine("Reservaton mointor stopped.");

            StopService(hostInventoryManager, "InventoryManager");
            StopService(hostRentalManager, "RentalManager");
            StopService(hostAccountManager, "AccountManager");
        }

        static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            RentalManager rentalManager = new RentalManager();

            Reservation[] reservations = rentalManager.GetDeadReservations();
            if (reservations != null)
            {
                foreach (Reservation reservation in reservations)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        rentalManager.CancelReservation(reservation.ReservationId);
                        scope.Complete();
                    }
                }
            }
        }

        static void StartService(SM.ServiceHost host, string serviceDescription)
        {
            host.Open();
            System.Console.WriteLine("Service '{0}' started.", serviceDescription);

            foreach (var endpoint in host.Description.Endpoints)
            {
                System.Console.WriteLine(string.Format("Listening on endpoint:"));
                System.Console.WriteLine(string.Format("Address: {0}", endpoint.Address.Uri.ToString()));
                System.Console.WriteLine(string.Format("Binding: {0}", endpoint.Binding.Name));
                System.Console.WriteLine(string.Format("Contract: {0}", endpoint.Contract.ConfigurationName));
            }

            System.Console.WriteLine();
        }

        static void StopService(SM.ServiceHost host, string serviceDescription)
        {
            host.Close();
            System.Console.WriteLine("Service '{0}' stopped.", serviceDescription);
        }
    }
}
