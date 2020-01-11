using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transports;
using IsaveSql;
using Orders;
using CLients;

using System.Data.SqlClient;
using System.Collections;
using Programm;
using System.Text.RegularExpressions;
using CSV;
using System.Xml;

namespace Main_abstract_class
{
  
    public abstract class Programm_beta
    {

    
         public abstract void createTransports();


        public abstract void createOrders();


       public abstract void createClients();
      
        

    }
    public class Progamm : Programm_beta
    {
        List<Transport> transports= new List<Transport>();
        List<Order> orders= new List<Order>();
        List<Client> clients;
        public Progamm()
        {


        }
        public void Delete_all()
        {
            transports[0].deleteall();
            orders[0].deleteall();

        }
        public void add_order(Order ord)
        {
            orders.Add(ord);
            ord.save();

        }
        public void add_transport(Transport tr)
        {
            transports.Add(tr);
            tr.save();
        }
        public void clear_transports()
        {
            transports.Clear();
        }


    
        public override void createTransports()
        {
            ConnectToSQL tosql = new ConnectToSQL();
            List<ArrayList> lists = tosql.Read("Transports");

            //({ maxVolume},{ curVolume},{ maxCap},{ curCap},{ trId},{ t_begin},{ t_end},{ cost},{ coffiecent}
            foreach (var item in lists)
            {
                Transport tr = new Transport(Convert.ToInt32(item[1]), Convert.ToInt32(item[3]), Convert.ToInt32(item[8]));
                tr.cost = Convert.ToInt32(item[7]);
                tr.curCap = Convert.ToInt32(item[4]);
                tr.curVolume = Convert.ToInt32(item[2]);
                tr.t_begin = Convert.ToString(item[5]);
                tr.t_end = Convert.ToString(item[6]);
                tr.Id = Convert.ToInt32(item[0]);

                transports.Add(tr);
            }
      

        }
        public void Write_transports(string filename)
        { 
             // Создаем объектную модель
            XmlDocument doc = new XmlDocument();

        //Создаем заголовок XML 
        XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", null, null);

        // Создаем основную ноду
        XmlElement node = doc.CreateElement("transports");
            foreach (var item in transports)
            {
                // Создаем ноду для машины
                XmlElement xmlcar = doc.CreateElement("transport");

                // Создаем атрибут цвета
                XmlElement xmlvolume = doc.CreateElement("volume");
                xmlvolume.InnerText = item.maxVolume.ToString();
                xmlcar.AppendChild(xmlvolume);

                XmlElement xmlcvol = doc.CreateElement("cvol");
                xmlcvol.InnerText = Convert.ToString(item.curVolume);
                xmlcar.AppendChild(xmlcvol);
                // Создаем ноду модели
                XmlElement xmlweight = doc.CreateElement("weight");
        xmlweight.InnerText = item.maxCap.ToString();
                xmlcar.AppendChild(xmlweight);

                XmlElement xmlccap = doc.CreateElement("ccap");
                xmlccap.InnerText = Convert.ToString(item.curCap);
                xmlcar.AppendChild(xmlccap);

                // Создаем ноду года
                XmlElement xmlcoef = doc.CreateElement("coef");
        xmlcoef.InnerText = Convert.ToString(item.coffiecent);
                xmlcar.AppendChild(xmlcoef);

                XmlElement xmltbeg = doc.CreateElement("tbeg");
                xmltbeg.InnerText = Convert.ToString(item.t_begin);
                xmlcar.AppendChild(xmltbeg);
                XmlElement xmltend = doc.CreateElement("tend");
                xmltend.InnerText = Convert.ToString(item.t_end);
                xmlcar.AppendChild(xmltend);


                // Добавляем ноду машины в ноды списка
                node.AppendChild(xmlcar);
            }
    // Добавляем созданную структуру в корень
    doc.AppendChild(node);
            doc.InsertBefore(xmldecl, node);
            doc.Save(filename);
        }
        public void Tr_info()

        {
            foreach (var item in transports)
            {
                item.getInfo();
            }
        }
        public void createTransports(string filename)
        {
            // Создаем объектную модель
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            // Выбираем необходимый список нод
            XmlNodeList list = doc.GetElementsByTagName("transport");
            foreach (XmlElement elem in list)
            {
                XmlNodeList volumelist = elem.GetElementsByTagName("volume");
                XmlNodeList weightlist = elem.GetElementsByTagName("weight");
                XmlNodeList coefficentlist = elem.GetElementsByTagName("coef");
                XmlNodeList cvolumelist = elem.GetElementsByTagName("cvol");
                XmlNodeList cweightlist = elem.GetElementsByTagName("ccap");
                XmlNodeList tbeginlist = elem.GetElementsByTagName("tbeg");
                XmlNodeList tendlist = elem.GetElementsByTagName("tend");



                Transport tmp = new Transport(Convert.ToInt32(volumelist.Item(0).InnerText), Convert.ToInt32(weightlist.Item(0).InnerText), Convert.ToInt32(coefficentlist.Item(0).InnerText));
                tmp.curVolume = Convert.ToInt32(cvolumelist.Item(0).InnerText);
                tmp.curCap = Convert.ToInt32(cweightlist.Item(0).InnerText);
                tmp.t_begin = Convert.ToString(tbeginlist.Item(0).InnerText);
                tmp.t_end = Convert.ToString(tendlist.Item(0).InnerText);


                this.transports.Add(tmp);
                tmp.save();
            }
        }


    
  

        public override void createOrders()
        {
            ConnectToSQL tosql = new ConnectToSQL();
            List<ArrayList> lists = tosql.Read("Orders");

            //({a_order_id},{volume},{weight},{o_begin},{o_end}
            foreach (var item in lists)
            {
                Order ord = new Order( Convert.ToInt32(item[1]), Convert.ToInt32(item[2]), Convert.ToString(item[4]), Convert.ToString(item[3]));
                ord.a_order_id = Convert.ToInt32(item[0]);
                orders.Add(ord);
            }
        }
        public void createOrders(string filename)



        {

            // Создаем объектную модель
            XmlDocument doc = new XmlDocument();
        doc.Load(filename);

            // Выбираем необходимый список нод
            XmlNodeList list = doc.GetElementsByTagName("order");
            foreach (XmlElement elem in list)
            {
                XmlNodeList volumelist = elem.GetElementsByTagName("volume");
        XmlNodeList weightlist = elem.GetElementsByTagName("weight");
       
        XmlNodeList obeginlist = elem.GetElementsByTagName("obeg");
        XmlNodeList oendlist = elem.GetElementsByTagName("oend");



        Order tmp = new Order(Convert.ToInt32(volumelist.Item(0).InnerText), Convert.ToInt32(weightlist.Item(0).InnerText), Convert.ToString(obeginlist.Item(0).InnerText), Convert.ToString(oendlist.Item(0).InnerText));


                this.orders.Add(tmp);
                tmp.save();
       
            }
}

        public void order_info()
        {
            foreach (var item in orders)
            {
                item.GetInfo();
            }
        }
public void Write_orders(string filename)
        {
            // Создаем объектную модель
            XmlDocument doc = new XmlDocument();

            //Создаем заголовок XML 
            XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", null, null);

            // Создаем основную ноду
            XmlElement node = doc.CreateElement("orders");
            foreach (var item in orders)
            {
                // Создаем ноду для машины
                XmlElement xmlcar = doc.CreateElement("order");

                // Создаем атрибут цвета
                XmlElement xmlvolume = doc.CreateElement("volume");
                xmlvolume.InnerText = item.getWeight().ToString();
                xmlcar.AppendChild(xmlvolume);

            
                // Создаем ноду модели
                XmlElement xmlweight = doc.CreateElement("weight");
                xmlweight.InnerText = item.getWeight().ToString();
                xmlcar.AppendChild(xmlweight);

               

                XmlElement xmltbeg = doc.CreateElement("obeg");
                xmltbeg.InnerText = Convert.ToString(item.Get_begin());
                xmlcar.AppendChild(xmltbeg);
                XmlElement xmltend = doc.CreateElement("oend");
                xmltend.InnerText = Convert.ToString(item.Get_end());
                xmlcar.AppendChild(xmltend);


                // Добавляем ноду машины в ноды списка
                node.AppendChild(xmlcar);
            }
            // Добавляем созданную структуру в корень
            doc.AppendChild(node);
            doc.InsertBefore(xmldecl, node);
            doc.Save(filename);
        }


        public override void createClients()
        {
            ConnectToSQL tosql = new ConnectToSQL();
            List<ArrayList> lists = tosql.Read("Persons");
            foreach (var item in lists)
            {
                Client client = new Client( Convert.ToString(item[1]),Convert.ToString(item[2]), Convert.ToString(item[3]));
                client.order_id = Convert.ToInt32 (item[4]);
                clients.Add(client);
            }
        }


        public  void makeNewOrder(Order item)
        {

            foreach (var item_t in transports)
            {
                if (item_t.Order(item))
                {

                    item.delete();
                    item_t.update();
                    Console.WriteLine("success");
                    break;
                }
            }

        }
        public void CheckOrders()

        {
            foreach (var item1 in orders)
            {


                foreach (var item_t in transports)
                {
                    if (item_t.Order(item1))
                    {
                        item1.delete();
                        item_t.update();
                        Console.WriteLine("success");
                        break;
                    }

                }
                this.check_transport();
                this.clear_transports();
                this.createTransports();



            }

        }
        public void check_transport()

        {
            foreach (var item in transports)
            { double t =Convert.ToDouble(item.maxCap) * 0.8;
                double p = Convert.ToDouble(item.maxVolume) * 0.8;
                if (t-Convert.ToDouble(item.curCap)  < 0 ||p- Convert.ToDouble(item.curVolume)<0)
                {
                    item.delete();
                }
            }
        }

   
    }


    class programm_for_client
    {
        int client_id;
        int order_id;
        public void makeOrder()
        {



        }


    }
}



namespace Transports
{
    public class Transport : IsaveSql.IsaveSql
    {
        public int maxVolume { get ; set; }
        public int curVolume { get; set; }
        public int maxCap { get; set; }
        public int curCap { get; set; }
        public int Id { get; set; }
        public string t_begin { get; set; }
        public string t_end { get; set; }
        public int cost { get; set; }
        public int coffiecent {get; set;}

        public Transport( int vol, int w, int k)
        {
            coffiecent = k;
            maxVolume = vol;
            maxCap = w;
            curCap = curVolume = 0;
            this.Set_cost();
            
            ////////}
        }
        public void update()
        {
            ConnectToSQL toSQL = new ConnectToSQL();
            this.update_cost();
            toSQL.Update($"update Transports set curVolume={curVolume},curCap={curCap},t_begin='{t_begin}',t_end='{t_end}' where Id={Id}");
        }
        private void Set_cost()
        {
            ConnectToSQL toSQL = new ConnectToSQL();
            int l = toSQL.get_lenght(t_begin, t_end);

            cost = ((maxVolume - curVolume) + (maxCap - curCap)) * coffiecent * l;


        }
        private void update_cost()
        {
            ConnectToSQL toSQL = new ConnectToSQL();
            int l = toSQL.get_lenght(t_begin,t_end);

            cost = ((maxVolume - curVolume) + (maxCap - curCap)) * coffiecent*l;

        }

        public bool Order(Order ord)
        {
            if (checkVolume(ord)==checkWeight(ord)&& checkWeight(ord)!=false)
            {
                if (curCap == 0 && curVolume == 0)
                {
                    First_order(ord);
                    update_cost();
                    return true;

                }
                if (checkVolume(ord) == checkWeight(ord) == checkFromTo(ord) == true)
                {
                    curCap += ord.getWeight();
                    curVolume += ord.getVolume();
                    update_cost();
                    return true;
                }
                return false;
            }
            return false;
        }
        public bool First_order(Order order)
        {
            this.t_begin = order.getFrom();
            this.t_end = order.getTo();
            this.curCap = order.getWeight();
            this.curVolume = order.getVolume();
            return true;

        }

        public bool checkWeight(Order ord)
        {
            if (curCap + ord.getWeight() > maxCap) { return false; }
            else { return true; }

        }
        public bool checkVolume(Order ord)
        {
            if (curVolume + ord.getVolume() > maxVolume)
            { return false; }
            else
            { return true; }
        }
        public bool checkFromTo(Order ord)
        {
            if (t_begin == ord.getFrom() && t_end == ord.getTo()) { return true; }
            else { return false; }
        }
        public void getInfo()
        {
            Console.WriteLine($"max Volume of car: {maxVolume}m3 current Volume: {curVolume}m3");
            Console.WriteLine($"max Capacity of car: {maxCap}kg current Capacity: {curCap}kg");
            Console.WriteLine($"Transport ID: {Id}, cost:{cost}");
        }
      //public List<string> save()
      //  {
      //      List<string> quaries = new List<string>();

      //      quaries.Add($"Update  Transports (maxVolume,curVolume,maxCap,curCap, trId ,from, to) Values ({maxVolume},{curVolume},{maxCap},{curCap},{trId},{from},{to}) where id={trId} ");
           

      //      return quaries;
      //  }
       public List<string> save()
       {
           List<string> quaries = new List<string>();

           quaries.Add($"Insert  Transports (maxVolume,curVolume,maxCap,curCap,t_begin, t_end,t_cost,t_coffiecent) Values ({maxVolume},{curVolume},{maxCap},{curCap},'{t_begin}','{t_end}',{cost},{coffiecent}) ");

            ConnectToSQL toSQL = new ConnectToSQL();
            toSQL.write($"Insert  Transports (maxVolume,curVolume,maxCap,curCap,t_begin, t_end,t_cost,t_coffiecent) Values ({maxVolume},{curVolume},{maxCap},{curCap},'{t_begin}','{t_end}',{cost},{coffiecent}) ");
           return quaries;
       }
        public void delete()
        {

            ConnectToSQL toSQL = new ConnectToSQL();
            toSQL.Delete("Transports",Id);
        }

        public void deleteall()
        {

            ConnectToSQL toSQL = new ConnectToSQL();
            toSQL.DeleteAll("Transports");
        }

        public void load(List<ArrayList> arrays)
        {
            throw new NotImplementedException();
        }


        //  void IsaveSql.IsaveSql.load(List<ArrayList> arrays)
        //  {

        //  }
        //НАДО СОЗДАВАТЬ ГЛАВНЫЙ КЛАСС И ПИХАТЬ ЭТО ТУДА(не забыть дописать коэфицент)
    }
}



namespace Orders
{
    public  class Order: IsaveSql.IsaveSql
    {
        public int a_order_id; 
        private int volume;
        private int weight;
        private readonly string o_begin;
        private readonly string o_end;

        public Order( int vol, int w, string to1, string from1)
        {

            
            volume = vol;
            weight = w;
            o_end = to1;
            o_begin = from1;
        
        }
        public void delete()
        {

            ConnectToSQL toSQL = new ConnectToSQL();
            toSQL.Delete("Orders", getOrderId());
        }
        public string Get_begin()
        { return o_begin; }
        public string Get_end()
        { return o_end; }
        public void deleteall()
        {

            ConnectToSQL toSQL = new ConnectToSQL();
            toSQL.DeleteAll("Orders");
        }
        public int getOrderId()
        {
            return a_order_id;
        }
        public int getWeight()
        {
            return weight;
        }
        public int getVolume()
        {
            return volume;
        }

        public string getFrom()
        {
            return o_begin;
        }
        public string getTo() { return o_end; }
        public void GetInfo()
        {
            Console.WriteLine($"Order id: {a_order_id} Order Volume: {volume}m3 Weight: {weight}");
        }

        //public List<string> save()
        //{
        //    List<string> quaries = new List<string>();

        //    quaries.Add($"Update  Orders (order_id,volume,weight,to,from) Values ({a_order_id},{volume},{weight},{to},{from}) where id={a_order_id} ");


        //    return quaries;
        //}
        public List<string> save()
       {
           List<string> quaries = new List<string>();

           quaries.Add($"Insert  Orders (volume,weight,o_begin,o_end) Values ({volume},{weight},'{o_begin}','{o_end}') ");
            ConnectToSQL toSQL = new ConnectToSQL();
            toSQL.write($"Insert  Orders (volume,weight,o_begin,o_end) Values ({volume},{weight},'{o_begin}','{o_end}') ");

            return quaries;
        }

   

        public void load(List<ArrayList> arrays)
        {
            throw new NotImplementedException();
        }


    }


}

namespace CLients
{
    class Client : Person, IsaveSql.IsaveSql
    {
        public int order_id;

        public Client(String name, String surname, String patronymic) : base(name, surname, patronymic)
        {
            save();
        }



        public void load(List<ArrayList> arrays)
        {
            throw new NotImplementedException();
        }




        public List<string> save()
        {
            List<string> quaries = new List<string>();
            //"Insert  Clients (Name,Surname,Patrinomyc,) "
            String str="select * Persons";
    
         
                
           str=$"Insert  Persons (name,surname,patrinomyc,order_id) Values ('{Name}','{Surname}','{Patronymic}')" ;
                quaries.Add(str);
            
            ConnectToSQL toSQL = new ConnectToSQL();
            toSQL.write(str);

            return quaries;
        }

   

        //void IsaveSql.IsaveSql.load(List<ArrayList> arrays)
        //   {
        //       throw new NotImplementedException();
        //   }
        //НАДО СОЗДАВАТЬ ГЛАВНЫЙ КЛАСС И ПИХАТЬ ЭТО ТУДА
    }
}
 


class Person
{
    private string name;
    private string surname;
    private string patronymic;
    private int birthYear;
    private long iin;

    public Person()
    {
        name = surname = patronymic = "Неизвестно";
        birthYear = 0;
        iin = 0;
    }
    public Person(string name, string surname, string patronymic)
    {
        this.name = name;


        this.surname = surname;

            this.patronymic = patronymic;
        birthYear = 0;
        iin = 0;
    }
    public Person(int BirthYear, long IIN)
    {
        this.birthYear = BirthYear;
        this.iin = IIN;
    }


    public string GetInfo()
    {
        string str;
        str = name + " " + surname + " " + patronymic;
        return str;
    }
    public string GetInitials()
    {
        string str;
        str = surname + " " + name[0] + surname[0] + patronymic[0];
        return str;
    }
    public int GetAge()
    {
        int age = 0;

        age = DateTime.Today.Year - birthYear;

        return age;
    }



    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }
    public string Surname
    {
        get
        {
            return surname;
        }
        set
        {
            surname = value;
        }
    }
    public string Patronymic { get => patronymic; set => patronymic = value; }
    public int BirthYear
    {
        get
        {
            return birthYear;
        }
        set
        {
            birthYear = value;
        }
    }

    public long IIN
    {
        get
        {
            return iin;
        }
        set
        {
            iin = value;
        }

    }
  
}


namespace IsaveSql
{
    interface IsaveSql
    {
        List<string> save();
        void load(List<ArrayList> arrays);
    }

    class ConnectToSQL
    {

        public void write(string str)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "DESKTOP-KJL28DL\\TIMKAS_SERVER";   // update me
            builder.InitialCatalog = "TestSharp";
            builder.IntegratedSecurity = true;
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();



            StringBuilder sb = new StringBuilder();

            Console.Write("Inserting a new row into table, press any key to continue...");
            Console.ReadKey(true);
            sb.Clear();


            String sql = str;
            using (SqlCommand command = new SqlCommand(sql, connection))
            {


                sb.Append(str);
          
                int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) inserted");
                }
                

            }
        public void DeleteAll(string table)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "DESKTOP-KJL28DL\\TIMKAS_SERVER";   // update me
            builder.InitialCatalog = "TestSharp";
            builder.IntegratedSecurity = true;
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append($"DELETE FROM {table}");
            string sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) deleted");
            }


        }
        public int get_lenght(string from,string to)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "DESKTOP-KJL28DL\\TIMKAS_SERVER";   // update me
            builder.InitialCatalog = "TestSharp";
            builder.IntegratedSecurity = true;
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
            int l = 1;
            Console.WriteLine("Reading data from table, press any key to continue...");
            Console.ReadKey(true);
            String sql;
            sql = $"select Lenght from Cities where right_c = '{from}'and left_c = '{to}'";
            List<ArrayList> arrayLists = new List<ArrayList>();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                    
                        l = Convert.ToInt32(reader.GetValue(0));

                    }
                }
            }
            
            return l;


        }

        public void Update(string str)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "DESKTOP-KJL28DL\\TIMKAS_SERVER";   // update me
            builder.InitialCatalog = "TestSharp";
            builder.IntegratedSecurity = true;
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            StringBuilder sb = new StringBuilder();
            String sql = str;
            using (SqlCommand command = new SqlCommand(sql, connection))
            {


                sb.Append(str);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) inserted");
            }
        }
        public void Road()
        {
            Roads roads = new Roads();
            roads.ParseFromCSV((new ReadCSV()).ReadFileCSV("roads.csv"));
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "DESKTOP-KJL28DL\\TIMKAS_SERVER";   // update me
            builder.InitialCatalog = "TestSharp";
            builder.IntegratedSecurity = true;
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
            StringBuilder sb = new StringBuilder();
            foreach (var item in roads.ways)
            {


                sb.Clear();
                sb.Append($"insert Cities (left_c,right_c,lenght)values ('{item.Left}','{item.Right}',{item.Length})");
                string sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected + " row(s) inserted");
                }
            }

       
        }
        public void Delete(string table, int id)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "DESKTOP-KJL28DL\\TIMKAS_SERVER";   // update me
            builder.InitialCatalog = "TestSharp";
            builder.IntegratedSecurity = true;
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append($"DELETE FROM {table} WHERE Id = {id}");
            string sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
             
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) deleted");
            }
        }
            public List<ArrayList> Read(string table)
            {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "DESKTOP-KJL28DL\\TIMKAS_SERVER";   // update me
            builder.InitialCatalog = "TestSharp";
            builder.IntegratedSecurity = true;
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
            Console.WriteLine("Reading data from table, press any key to continue...");
                Console.ReadKey(true);
            String sql;
                sql = $"SELECT * FROM {table};";
                List<ArrayList> arrayLists = new List<ArrayList>();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            switch (table)
                            {
                            case "Orders":
                                    {
                                    ArrayList arr = new ArrayList();
                                    arr.Add(Convert.ToInt32(reader.GetValue(0)));
                                    arr.Add(Convert.ToInt32(reader.GetValue(1)));
                                    arr.Add(Convert.ToInt32(reader.GetValue(2)));
                                    arr.Add(Convert.ToString(reader.GetValue(3)));
                                    arr.Add(Convert.ToString(reader.GetValue(4)));
                              
                                    arrayLists.Add(arr);
                                    break;
                                }
                                case "Transports":
                                    {
                                    ArrayList arr = new ArrayList();
                                    arr.Add(Convert.ToInt32(reader.GetValue(0)));
                                    arr.Add(Convert.ToInt32(reader.GetValue(1)));
                                    arr.Add(Convert.ToInt32(reader.GetValue(2)));
                                    arr.Add(Convert.ToInt32(reader.GetValue(3)));
                                    arr.Add(Convert.ToInt32(reader.GetValue(4)));
                                    arr.Add(Convert.ToString(reader.GetValue(5)));
                                    arr.Add(Convert.ToString(reader.GetValue(6)));
                                    arr.Add(Convert.ToInt32(reader.GetValue(7)));
                                    arr.Add(Convert.ToInt32(reader.GetValue(8)));
                                    arrayLists.Add(arr);
                                        break;


                                    }
                                case "Clients":
                                    {

                                    ArrayList arr = new ArrayList();
                                    arr.Add(Convert.ToInt32(reader.GetValue(0)));
                                   arr.Add (Convert.ToString(reader.GetValue(1)));
                                    arr.Add(Convert.ToString(reader.GetValue(2)));
                                    arr.Add(Convert.ToString(reader.GetValue(3)));



                                    arrayLists.Add(arr);
                                        break;

                                    }
                                default:
                                    Console.WriteLine("Invalid selection. Please choose the correct table");
                                    break;


                            }
                        }
                        return arrayLists;

                    }
                }
            }
        }
    }

namespace CSV
{

    internal class Way
    {
        public string Left { get; set; }
        public string Right { get; set; }
        public uint Length { get; set; }

        public Way(string str)
        {
            Right = str;
        }

        public override string ToString()
        {
            return $"Между {Left} и {Right} - {Length}км.";
        }
    }

    internal class Roads
    {
        public List<Way> ways;

        public Roads()
        {
            ways = new List<Way>();
        }

        public void ParseFromCSV(string str)
        {
            if (str.Length > 0)
            {
                try
                {
                    // Преобразуем строку в массив строк таблицы.
                    ArrayList strs = ParseString(str, '\n');
                    // Строку таблицы делим на колонки.
                    ArrayList rows = new ArrayList();
                    foreach (string elem in strs)
                        rows.Add(ParseString(elem, ';'));
                    // Отделяем шапку таблицы.
                    ArrayList header = rows[0] as ArrayList;
                    for (int i = 1; i < rows.Count; i++)
                    {
                        // Извлекаем строку таблицы.
                        ArrayList row = rows[i] as ArrayList;
                        for (int j = 1; j < row.Count; j++)
                        {
                            // Заполняем расстояние.
                            Way way = new Way(FixString(row[0] as string));
                            way.Left = FixString(header[j] as string);
                            way.Length = Convert.ToUInt32(FixString(row[j] as string));
                            ways.Add(way);
                        }

                    }
                }
                catch
                {
                    Console.WriteLine("Несовместимый формат файла или файл поврежден.");
                }
            }
        }

        private ArrayList ParseString(string str, char separator)
        {
            ArrayList answer = new ArrayList();
            int pos = 0, prepos = 0;
            while (!(pos < 0))
            {
                pos = str.IndexOf(separator, pos + 1);
                if (pos < 0)
                {
                    answer.Add(str.Substring(prepos));
                }
                else
                {
                    answer.Add(str.Substring(prepos, (pos - prepos)));
                    prepos = pos;
                }
            }
            return answer;
        }

        private string FixString(string str)
        {
            Regex rex = new Regex(@"\W"); // Использование регулярных выражений.
            string spaseStr = rex.Replace(str, " "); // Заменяем символы на пробелы.
            return spaseStr.Trim(); // Удаляем не значащие пробелы.
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Way elem in ways)
                stringBuilder.Append($"{elem}\n");
            return stringBuilder.ToString();
        }
    }

    internal class ReadCSV
    {

        public string ReadFileCSV(string filename)
        {
            string text = "";
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(filename))
                {
                    text = file.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удаётся открыть файл по причине {ex.Message}");
            }
            return text;
        }
    }


}


namespace Programm
    {
        class Programm
        {
            static void Main(string[] args)
            {
        
            Transport tr1=new Transport(20, 20, 10);
            Transport tr2=new Transport(10, 30, 10);
            Transport tr3=new Transport(30, 30, 10);
            Order order = new Order(20, 20, "Алматы", "Астана");
            Order order1 = new Order(10, 10, "Алматы", "Астана");
            Order order2 = new Order(10, 10, "Алматы", "Астана");
            Order order3 = new Order(10, 10, "Алматы", "Астана");
            //Client client1 = new Client("Ivanov", "Ivan", "Ivanovich");
            //int k;
            //foreach (var item in transports)
            //{
            //    if (item.Order(order))
            //    {
            //        Console.WriteLine("success");
            //        break;
            //    }
            //}
            //foreach (var item in transports)
            //{
            //    item.getInfo();
            //}
            //foreach (var item in transports)
            //{
            //    if (item.Order(order1))
            //    {
            //        Console.WriteLine("success");
            //        break;
            //    }
            //}
            //foreach (var item in transports)
            //{
            //    item.getInfo();
            //}
            //foreach (var item in transports)
            //{
            //    if (item.Order(order2))
            //    {
            //        Console.WriteLine("success");
            //        break;
            //    }
            //}
            //foreach (var item in transports)
            //{
            //    item.getInfo();
            //}
            //foreach (var item in transports)
            //{
            //    if (item.Order(order3))
            //    {
            //        Console.WriteLine("success");
            //        break;
            //    }
            //}
            //foreach (var item in transports)
            //{
            //    item.getInfo();
            //}
            //Console.ReadKey();
            Main_abstract_class.Progamm programm = new Main_abstract_class.Progamm();
            //programm.add_order(order);
            //programm.add_order(order1);
            //programm.add_order(order2);
            //programm.add_order(order3);
            //programm.add_transport(tr1);
            //programm.add_transport(tr2);
            //programm.add_transport(tr3);
            //programm.createOrders("Orders.xml");
            //programm.createTransports("Transports.xml");
            programm.Tr_info();
            programm.order_info();
            //programm.Write_orders("Orders.xml");


            programm.createOrders();
            programm.createTransports();
            //programm.createOrders();


            //programm.Write_transports("Transports.xml");
            //programm.Delete_all();
            programm.CheckOrders();
            //ConnectToSQL toSQL = new ConnectToSQL();
            //toSQL.Road();

            //programm.GetTransports()[0].deleteall();
            //programm.GetOrders()[0].deleteall();

            Console.ReadKey();
            }


        
    }

    }
