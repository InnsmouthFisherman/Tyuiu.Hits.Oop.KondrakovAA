namespace review4
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Enterprise DevilMayCry = new Enterprise();
			Employee dante = new Employee("Dante", 999, "Demon Slayer");
			Supplier virgil = new Supplier("Pizza Supplier", "Virgil", 9);
			DevilMayCry.AddSupplier(virgil);
			DevilMayCry.AddEmployee(dante);
			DevilMayCry.CalculateTotalExpenses();
			Console.ReadKey();
		}
	}

	public interface IEnterprise
	{
		public void AddSupplier(ISupplier supplier);
		public void AddEmployee(IEmployee employee);
		public List<ISupplier> GetSuppliers();
		decimal CalculateTotalExpenses();
	}

	public interface ISupplier
	{
		public string GetName();
		public decimal GetCost();
		public string SupplierType { get; set; }
	}

	public interface IEmployee
	{
		public string GetFullName();
		public decimal GetSalary();
		public string Position { get; set; }
	}

	public class Employee: IEmployee 
	{
		private string Name;
		private decimal Salary;
		public string Position { get; set; }
		
		public Employee(string N, decimal S, string P) 
		{
			Name = N;
			Salary = S;
			Position = P;	
		}
		public string GetFullName()
		{
			return Name;
		}
		public decimal GetSalary()
		{
			return Salary;
		}

	}

	public class Supplier : ISupplier 
	{
		public string SupplierType { get; set; } 
		private string Name;
		private decimal Cost;

		public Supplier(string S, string N, decimal C)
		{
			SupplierType = S;
			Name = N;
			Cost = C;
		}
		public string GetName()
		{
			return Name;
		}

		public decimal GetCost() 
		{
			return Cost;
		}

	}

	public class Enterprise: IEnterprise
	{
		List<ISupplier> suppliers = [];
		List<IEmployee> employees = [];
		public void AddSupplier(ISupplier supplier)
		{
			suppliers.Add(supplier);
		}
		public void AddEmployee(IEmployee employee)
		{
			employees.Add(employee);	
		}
		public List<ISupplier> GetSuppliers()
		{
			return suppliers;
		}
		public decimal CalculateTotalExpenses()
		{
			decimal total = 0;
			Console.WriteLine("Список поставщиков: ");
			for (int i = 0; i < suppliers.Count; i++)
			{
				Console.WriteLine(suppliers[i].GetName() + " - Стоимость услуг: " + suppliers[i].GetCost());
				total += suppliers[i].GetCost();
			}
			Console.WriteLine("Список сотрудников: ");
			for (int i = 0; i < employees.Count; i++)
			{
				Console.WriteLine(employees[i].GetFullName() + " - Должность: " + employees[i].Position + " - Зарплата: " + employees[i].GetSalary());
				total += employees[i].GetSalary();
			}
			Console.WriteLine("Итого: " + total.ToString());
			return  total;	
		}
	}
}
