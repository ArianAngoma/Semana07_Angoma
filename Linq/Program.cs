using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    class Program
    {
        public static DataClasses1DataContext context = new DataClasses1DataContext();
        static void Main(string[] args)
        {
            var Pedidos = context.Pedidos.ToList();
            var Clientes = context.clientes.ToList();
            var DetallePedidos = context.detallesdepedidos.ToList();
            var Productos = context.productos.ToList();
            var Proveedores = context.proveedores.ToList();

            //Consulta01(Pedidos, Clientes);
            //Consulta02(Pedidos, Clientes);
            //Consulta03(Pedidos, Clientes, DetallePedidos);
            Consulta04(Productos, Proveedores);

            Console.Read();
        }

        static void IntroToLINQ()
        {
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

            var numQuery =
                from num in numbers
                where (num % 2) == 0
                select num;

            foreach(int num in numQuery)
            {
                Console.Write("{0,1} ", num);
            }
        }

        static void DataSource()
        {
            var queryAllCustomers = from cust in context.clientes select cust;

            foreach(var item in queryAllCustomers)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void Filtering()
        {
            var queryLondonCustomers = from cust in context.clientes
                where cust.Ciudad == "Londres"
                select cust;

            foreach(var item in queryLondonCustomers)
            {
                Console.WriteLine(item.Ciudad);
            }
        }

        static void Ordering()
        {
            var queryLondonCustomers3 = 
                from cust in context.clientes
                where cust.Ciudad == "London"
                orderby cust.NombreCompañia ascending
                select cust;

            foreach(var item in queryLondonCustomers3)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void Grouping()
        {
            var queryCustomersByCity =
                from cust in context.clientes
                group cust by cust.Ciudad;

            foreach(var customerGroup in queryCustomersByCity)
            {
                Console.WriteLine(customerGroup.Key);

                foreach(clientes customer in customerGroup)
                {
                    Console.WriteLine("  {0}", customer.NombreCompañia);
                }
            }
        }

        static void Grouping2()
        {
            var custQuery =
                from cust in context.clientes
                group cust by cust.Ciudad into custGroup
                where custGroup.Count() > 2
                orderby custGroup.Key
                select custGroup;

            foreach(var item in custQuery)
            {
                Console.WriteLine(item.Key);
            }
        }

        static void Joining()
        {
            var innerJoinQuery =
                from cust in context.clientes
                join dist in context.Pedidos on cust.idCliente equals dist.IdCliente
                select new { CustomerName = cust.NombreCompañia, DistributorName = dist.PaisDestinatario };

            foreach(var item in innerJoinQuery)
            {
                Console.WriteLine(item.CustomerName);
            }
        }

        // FUNCIONES LAMBDA
        static void IntroToLINQLAMBDA()
        {
            //Pasos:
            // 1. Data Source
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

            // Lambda expression
            var nums = numbers.Where(n => n % 2 == 0);

            foreach (int num in nums)
            {
                Console.Write("{0,1} ", num);
            }

        }

        static void DataSourceLAMBDA()
        {

            var Clientes = context.clientes;

            foreach (var Cliente in Clientes)
            {
                Console.WriteLine(Cliente.NombreCompañia);
            }

        }

        static void OrderingLAMBDA()
        {
            var queryLondonCustomers3 = context.clientes.Where(c => c.Ciudad == "Londres")
                                            .OrderBy(c => c.NombreCompañia);

            foreach (var item in queryLondonCustomers3)
            {
                Console.WriteLine(item.NombreCompañia);
            }

        }

        static void GroupingLAMBDA()
        {
            var queryCustomersByCity = context.clientes.GroupBy(c => c.Ciudad);

            foreach (var customerGroup in queryCustomersByCity)
            {
                Console.WriteLine(customerGroup.Key);
                foreach (clientes customer in customerGroup)
                {
                    Console.WriteLine("     {0}", customer.NombreCompañia);
                }
            }

        }

        static void Grouping2LAMBDA()
        {
            var custQuery = context.clientes.GroupBy(c => c.Ciudad).Where(c => c.Count() > 2).OrderBy(c => c.Key);

            foreach (var item in custQuery)
            {
                Console.WriteLine(item.Key);
            }

        }

        static void JoiningLAMBDA()
        {
            var queryJoin = context.clientes.Join(context.Pedidos,
              cust => cust.idCliente,
              dist => dist.IdCliente,
              (cust, dist) => new
              {
                  CustomerName = cust.NombreCompañia,
                  DistributorName = dist.PaisDestinatario

              });

            foreach (var item in queryJoin)
            {
                Console.WriteLine(item.CustomerName);
            }

        }


        //Desarrollo del laboratorio
        static void Consulta01(List<Pedidos> pedidos, List<clientes> clientes)
        {
            var consulta01 = from ped in pedidos
                             join cli in clientes on ped.IdCliente equals cli.idCliente
                             where ped.FechaPedido < DateTime.Today.AddYears(-25) 
                             select new { CustomerName = cli.NombreCompañia };

            foreach (var item in consulta01)
            {
                Console.WriteLine(item.CustomerName);
            }
        }

        static void Consulta02(List<Pedidos> pedidos, List<clientes> clientes)
        {
            var consulta02 = from ped in pedidos
                             join cli in clientes on ped.IdCliente equals cli.idCliente
                             where ped.IdCliente.Count() > 2
                             select new { CustomerName = cli.NombreCompañia };

            foreach (var item in consulta02)
            {
                Console.WriteLine(item.CustomerName);
            }
        }

        static void Consulta03(List<Pedidos> pedidos, List<clientes> clientes, List<detallesdepedidos> detallesdepedidos)
        {
            var consulta03 = from ped in pedidos
                             join det in detallesdepedidos on ped.IdPedido equals det.idpedido
                             join cli in clientes on ped.IdCliente equals cli.idCliente
                             where det.cantidad * det.preciounidad > 200
                             select new { CustomerName = cli.NombreCompañia };

            foreach (var item in consulta03)
            {
                Console.WriteLine(item.CustomerName);
            }
        }

        static void Consulta04(List<productos> productos, List<proveedores> proveedores)
        {

            var lista = proveedores.Where(x => x.productos.Count() > 2);

            foreach (var item in lista)
            {
                Console.WriteLine(item.nombreCompañia);
            }
        }

        static void Consulta05(List<Pedidos> pedidos, List<detallesdepedidos> detallesdepedidos, List<productos> productos)
        {

            var lista = from ped in pedidos
                        join det in detallesdepedidos on ped.IdPedido equals det.idpedido
                        join pro in productos on det.idproducto equals pro.idproducto
                        where det.idpedido.Count() > 3
                        select new { ProductoName = pro.nombreProducto };

            foreach (var item in lista)
            {
                Console.WriteLine(item.ProductoName);
            }
        }
    }
}
