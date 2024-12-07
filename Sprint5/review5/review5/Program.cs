using System;
using System.Collections.Generic;
using System.Linq;

namespace review5
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Company LethalCompany = new Company("Lethal C.O.");
			Employee vacok = new Employee("Vacok", "Lohov");
			LethalCompany.AddEmployee(vacok);

			Customer customer1 = new Customer("John", "Doe", "123-456-7890");
			Customer customer2 = new Customer("Jane", "Smith", "987-654-3210");

			Order pizdel1 = new Order(1, "Pizza Delivery", customer1);
			Order pizdel2 = new Order(2, "Burger Delivery", customer2);

			LethalCompany.AddOrder(pizdel1, vacok);
			LethalCompany.AddOrder(pizdel2, vacok);

			pizdel1.OrderStatusChanged += LethalCompany.HandleOrderStatusChanged;
			pizdel2.OrderStatusChanged += LethalCompany.HandleOrderStatusChanged;

			LethalCompany.StatusUpdate(pizdel1);
			LethalCompany.StatusUpdate(pizdel2);

			var createdOrders = LethalCompany.GetOrdersByStatus(OrderStatus.Created);
			var inProgressOrders = LethalCompany.GetOrdersByStatus(OrderStatus.InProgress);
			var completedOrders = LethalCompany.GetOrdersByStatus(OrderStatus.Completed);

			Console.WriteLine("Заказы со статусом 'Created':");
			foreach (var order in createdOrders)
			{
				Console.WriteLine($"Заказ {order.id}: {order.status}");
			}

			Console.WriteLine("Заказы со статусом 'InProgress':");
			foreach (var order in inProgressOrders)
			{
				Console.WriteLine($"Заказ {order.id}: {order.status}");
			}

			Console.WriteLine("Заказы со статусом 'Completed':");
			foreach (var order in completedOrders)
			{
				Console.WriteLine($"Заказ {order.id}: {order.status}");
			}

			var ordersByCustomer1 = LethalCompany.GetOrdersByCustomer(customer1);
			var ordersByCustomer2 = LethalCompany.GetOrdersByCustomer(customer2);

			Console.WriteLine("Заказы от заказчика John Doe:");
			foreach (var order in ordersByCustomer1)
			{
				Console.WriteLine($"Заказ {order.id}: {order.status}");
			}

			Console.WriteLine("Заказы от заказчика Jane Smith:");
			foreach (var order in ordersByCustomer2)
			{
				Console.WriteLine($"Заказ {order.id}: {order.status}");
			}

			Console.ReadKey();
		}
	}

	public class Company
	{
		private string Name;
		private List<Employee> employees = new List<Employee>();
		private List<Order> orders = new List<Order>();

		public Company(string n) { Name = n; }
		public void AddEmployee(Employee e)
		{
			employees.Add(e);
		}
		public void AddOrder(Order o, Employee e)
		{
			o.id = orders.Count + 1;
			o.AssignedEmployee = e;
			StatusUpdate(o);
			orders.Add(o);
		}
		public void OrderComplete(Order o)
		{
			o.status = OrderStatus.Completed;
			o.AssignedEmployee.ordersProcessed += 1;
		}

		public void StatusUpdate(Order o)
		{
			o.ChangeStatus(o.status + 1); 
		}

		public void HandleOrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
		{
			Console.WriteLine($"Статус заказа {order.id} изменен с {oldStatus} на {newStatus}");
		}

		public List<Order> GetOrdersByStatus(OrderStatus status)
		{
			return orders.Where(o => o.status == status).ToList();
		}

		public List<Order> GetOrdersByCustomer(Customer customer)
		{
			return orders.Where(o => o.Customer == customer).ToList();
		}
	}

	public class Employee
	{
		private string Name;
		private string LastName;
		public int ordersProcessed = 0;

		public Employee(string n, string l) { Name = n; LastName = l; }
	}

	public enum OrderStatus
	{
		Created,
		InProgress,
		Completed,
	}

	public class Order
	{
		public int id { get; set; }
		private string Description;
		public Employee AssignedEmployee = null;
		public OrderStatus status;
		private DateTime CreatedAt;
		public Customer Customer;

		public delegate void OrderStatusChangedEventHandler(Order order, OrderStatus oldStatus, OrderStatus newStatus);

		public event OrderStatusChangedEventHandler OrderStatusChanged;

		public Order(int i, string d, Customer customer)
		{
			id = i;
			Description = d;
			CreatedAt = DateTime.Now;
			status = OrderStatus.Created;
			Customer = customer;
		}
		public void ChangeStatus(OrderStatus newStatus)
		{
			OrderStatus oldStatus = status;
			status = newStatus;
			OnOrderStatusChanged(oldStatus, newStatus);
		}

		protected virtual void OnOrderStatusChanged(OrderStatus oldStatus, OrderStatus newStatus)
		{
			OrderStatusChanged?.Invoke(this, oldStatus, newStatus);
		}
	}

	public class Customer
	{
		private string Name;
		private string LastName;
		private string PhoneNumber;

		public Customer(string n, string l, string num) { Name = n; LastName = l; PhoneNumber = num; }
	}
}