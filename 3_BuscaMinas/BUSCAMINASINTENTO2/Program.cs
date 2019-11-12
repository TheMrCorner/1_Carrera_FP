//Práctica numero 3 BUSCAMINAS
//Alumno:
//Alejandro Marín Pérez

using System;
using System.IO;

namespace PacMan
{
	class Program
	{

		//Tipos de casillas que se van a usar en el juego
		public struct Casilla
		{
			public bool mina;

			//'o' casilla sin descubrir, 'x' marcada, '*' mina
			//'0'-'8' número de minas que rodean una casilla
			public char estado;
		}
		public struct Tablero
		{
			public Casilla[,] cas;
			public int posX, posY;
		}

		public struct pares
		{
			public int x;
			public int y;
		}

		//Array con los colores para los números
		static ConsoleColor[] colors = {ConsoleColor.Cyan, ConsoleColor.Green,
			ConsoleColor.Yellow, ConsoleColor.Magenta, ConsoleColor.Red, ConsoleColor.Gray, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta};

		//Método que crea el tablero de juego
		public static Tablero creaTablero(int fils, int cols, int numMinas)
		{
			Tablero tab;
			int minasTablero = 0;

			tab.cas = new Casilla[fils, cols];

			//Rellenamos el tablero con casillas tapadas
			for (int j = 0; j < tab.cas.GetLength(0); j++)
				for (int i = 0; i < tab.cas.GetLength(1); i++)
				{
					tab.cas[j, i].estado = 'o';
				}

			//Situamos las bombas
			while (minasTablero < numMinas)
			{
				Random rnd = new Random();
				int j = rnd.Next(0, tab.cas.GetLength(0) - 1);
				int i = rnd.Next(0, tab.cas.GetLength(1) - 1);

				if (tab.cas[j, i].mina == false)
				{
					tab.cas[j, i].mina = true;
					minasTablero++;
				}
			}

			tab.posX = 0;
			tab.posY = 0;

			return tab;

		}

		//Dibujamos el tablero en consola
		public static void Dibuja(Tablero tab, bool bomba)
		{
			if (!bomba)
			{
				for (int j = 0; j < tab.cas.GetLength(0); j++)
				{
					for (int i = 0; i < tab.cas.GetLength(1); i++)
					{
						if (i == tab.posX && j == tab.posY)
						{
							Console.BackgroundColor = ConsoleColor.White;
							Console.ForegroundColor = ConsoleColor.Black;
							if (tab.cas[j, i].estado == '0')
								Console.Write(" ");
							else
								Console.Write(tab.cas[j, i].estado);
							Console.ForegroundColor = ConsoleColor.White;
							Console.BackgroundColor = ConsoleColor.Black;
						}
						else
						{
							if (tab.cas[j, i].estado == 'o' || tab.cas[j, i].estado == '*' || tab.cas[j, i].estado == 'x')
								Console.Write(tab.cas[j, i].estado);
							else if (tab.cas[j, i].estado == '0')
							{
								Console.Write(" ");
							}
							else
							{
								int color = (int)char.GetNumericValue(tab.cas[j, i].estado);
								Console.ForegroundColor = colors [color - 1];
								Console.Write(tab.cas[j, i].estado);
								Console.ForegroundColor = ConsoleColor.White;
							}
						}
					}
					Console.WriteLine();
				}
			}
			else
			{
				for (int j = 0; j < tab.cas.GetLength(0); j++)
				{
					for (int i = 0; i < tab.cas.GetLength(1); i++)
					{
						if (tab.cas[j, i].mina)
						{
							Console.BackgroundColor = ConsoleColor.Red;
							Console.ForegroundColor = ConsoleColor.Black;
							Console.Write('*');
							Console.ForegroundColor = ConsoleColor.White;
							Console.BackgroundColor = ConsoleColor.Black;
						}
						else
						{
							if (tab.cas[j, i].estado == 'o' || tab.cas[j, i].estado == '*' || tab.cas[j, i].estado == 'x')
								Console.Write(tab.cas[j, i].estado);
							else if (tab.cas[j, i].estado == '0')
							{
								Console.Write(" ");
							}
							else
							{
								int color = (int)char.GetNumericValue(tab.cas[j, i].estado);
								Console.ForegroundColor = colors[color - 1];
								Console.Write(tab.cas[j, i].estado);
								Console.ForegroundColor = ConsoleColor.White;
							}
						}
					}
					Console.WriteLine();
				}
			}
		}

		public static void DescubreAdyacentes(Tablero tab, int x, int y)
		{
			pares[] pendientes = new pares[tab.cas.GetLength(1) * tab.cas.GetLength(0)];
			int act = 0;         //referencia que indica la casilla que estamos explorando
			int sigLibre = 1;    //Indica la siguiente posición libre del array

			pendientes[0].x = x;
			pendientes[0].y = y;

			//Exploramos todo el array de pendientes
			while (act < sigLibre)
			{
				//Comprobamos que esa casilla esta cubierta y no es una mina
				if (tab.cas[pendientes[act].y, pendientes[act].x].estado == 'o')
				{
					//Asignamos el número de minas que la rodean al estado de la casilla
					tab.cas[pendientes[act].y, pendientes[act].x].estado = MinasAlRededor(tab, pendientes[act].x, pendientes[act].y);

					//Si no hay minas al rededor de esa casilla, añadimos las casillas adyacentes a pendientes
					if (tab.cas[pendientes[act].y, pendientes[act].x].estado == '0')
					{
						for (int j = Math.Max(0, pendientes[act].y - 1); j <= Math.Min(pendientes[act].y + 1, tab.cas.GetLength(0) - 1); j++)
							for (int i = Math.Max(0, pendientes[act].x - 1); i <= Math.Min(pendientes[act].x + 1, tab.cas.GetLength(1) - 1); i++)
							{
								//Si la casilla no está en el array la añadimos
								if (!EstaEnPendientes(pendientes, i, j, sigLibre))
								{
									pendientes[sigLibre].x = i;
									pendientes[sigLibre].y = j;

									sigLibre++;
								}
							}
					}
				}

				act++;
			}
		}

		//Mira el número de minas que rodea a una casilla
		static char MinasAlRededor(Tablero tab, int x, int y)
		{
			int minas = 0;
			char numMinas = ' ';

			for (int j = Math.Max(0, y - 1); j <= Math.Min(y + 1, tab.cas.GetLength(0) - 1); j++)
				for (int i = Math.Max(0, x - 1); i <= Math.Min(x + 1, tab.cas.GetLength(1) - 1); i++)
				{
					if (tab.cas[j, i].mina)
						minas++;
				}

			numMinas = char.Parse(minas.ToString());

			return numMinas;
		}


		//Comprobamos que la casilla no esta en el array de pendientes para añadirla
		static bool EstaEnPendientes(pares[] pendientes, int x, int y, int sigLibre)
		{
			int i = 0;
			bool estaEnArray = false;

			while (i < sigLibre && !estaEnArray)
			{
				if (pendientes[i].x == x && pendientes[i].y == y)
					estaEnArray = true;
				else
					i++;
			}

			return estaEnArray;
		}

		public static bool ClickCasilla(ref Tablero tab)
		{
			bool fin = false;

			if (tab.cas[tab.posY, tab.posX].estado == 'o')
			{
				if (tab.cas[tab.posY, tab.posX].mina)
				{
					tab.cas[tab.posY, tab.posX].estado = '*';
					fin = true;
				}
				else
					DescubreAdyacentes(tab, tab.posX, tab.posY);
			}

			return fin;
		}

		public static char LeeInput()
		{
			char inPut = 'j';

			if (Console.KeyAvailable)
			{
				//Guardamos el valor de la tecla en un string
				string tecla = Console.ReadKey().Key.ToString();

				//Comprobamos la dirección con el valor del string tecla
				//Asginamos a h la dirección
				switch (tecla)
				{
				//Movemos arriba
				case ("UpArrow"):
					inPut = 'u';
					break;
					//Movemos abajo
				case ("DownArrow"):
					inPut = 'd';
					break;
					//Movemos izquierda
				case ("LeftArrow"):
					inPut = 'l';
					break;
					//Movemos derecha
				case ("RightArrow"):
					inPut = 'r';
					break;
					//Descrubrimos
				case ("Spacebar"):
					inPut = 'c';
					break;
					//MarcamosBomba
				case ("Enter"):
					inPut = 'x';
					break;
					//Salimos del juego
				case ("Escape"):
					inPut = 'q';
					break;
				default:
					inPut = 'j';
					break;
				}
			}

			return inPut;
		}

		public static bool ProcesaInput(ref Tablero tab, char inPut)
		{
			int aux = 0;
			bool mina = false;

			switch (inPut)
			{
			//Movemos arriba
			case ('u'):
				aux = tab.posY;
				if (aux-- > 0)
					tab.posY--;
				break;
				//Movemos abajo
			case ('d'):
				aux = tab.posY;
				if (aux++ < tab.cas.GetLength(0) - 1)
					tab.posY++;
				break;
				//Movemos izquierda
			case ('l'):
				aux = tab.posX;
				if (aux-- > 0)
					tab.posX--;
				break;
				//Movemos derecha
			case ('r'):
				aux = tab.posX;
				if (aux++ < tab.cas.GetLength(1) - 1)
					tab.posX++;
				break;
			case ('c'):
				mina = ClickCasilla(ref tab);
				break;
			case ('x'):
				tab.cas[tab.posY, tab.posX].estado = 'x';
				break;
			case ('q'):
				Guardar(tab);
				break;
			}

			return mina;
		}

		public static bool Terminado(Tablero tab)
		{
			int minasMarcadas = 0;
			int numMinas = 0;
			bool destapadas = true;

			//Recorremos el tablero buscando las minas marcadas que hay
			for (int j = 0; j < tab.cas.GetLength(0) - 1; j++)
			{
				for (int i = 0; i < tab.cas.GetLength(1) - 1; i++)
				{
					if (tab.cas [j, i].estado == 'o')
						destapadas = false;

					if (tab.cas[j, i].mina)
					{
						if (tab.cas[j, i].estado == 'x')
							minasMarcadas++;

						numMinas++;
					}
				}
			}

			return ((minasMarcadas == numMinas) && destapadas);
		}

		public static void Jugar()
		{
			//variables del juego
			int ancho = 0;
			int alto = 0;
			bool fin = false;
			bool bomba = false;
			bool win = false;
			int numMinas = 0;
			Tablero tab;

			//Variables del menú
			bool cargar = false;
			bool guardar = false;

			//Menú principal
			Console.WriteLine("Cargar partida guardada: 1");
			Console.WriteLine("Comenzar Partida nueva: 2");
			int menu = int.Parse(Console.ReadLine());

			if (menu == 1)
				cargar = true;

			if (!cargar)
			{

				//Pedimos los datos del tablero al jugador
				Console.Write("Ancho del tablero: ");
				ancho = int.Parse(Console.ReadLine());

				Console.Write("Largo del tablero: ");
				alto = int.Parse(Console.ReadLine());

				while (numMinas < 1 || numMinas > ancho * alto) //El número de minas no puede superar el máximo número de casillas
				{
					Console.Write("Número de minas: ");
					numMinas = int.Parse(Console.ReadLine());
				}


				tab = creaTablero(ancho, alto, numMinas);
			}

			else
				tab = Cargar();

			Console.Clear();
			Dibuja(tab, bomba);


			//Bucle de juego
			while (!fin)
			{
				char inPut = LeeInput();
				bomba = ProcesaInput(ref tab, inPut);

				if (inPut != 'j' && !bomba)
				{
					if (inPut == 'q')
					{
						fin = true;
						guardar = true;
					}
					else
					{
						Console.Clear();
						Dibuja(tab, bomba);
					}
				}
				else if (bomba)
				{
					Console.Clear();
					Dibuja(tab, bomba);
					fin = true;
					win = false;
				}

				if (!fin) {
					win = Terminado (tab);

					if (win)
						fin = true;
				}
			}

			if (!guardar)
			{
				if (win)
				{
					Console.WriteLine("Has ganado!");
				}
				else
					Console.WriteLine("Has perdido!");
			}
			else
			{
				Console.WriteLine("Partida guardada");
				Console.WriteLine("Enter para salir");
				Console.ReadLine();
			}

		}

		public static void Guardar(Tablero tab)
		{
			StreamWriter salida;

			salida = new StreamWriter("guardado.txt");

			salida.WriteLine("Archivo de guardado:");

			salida.WriteLine(tab.cas.GetLength(0));
			salida.WriteLine(tab.cas.GetLength(1));

			for (int j = 0; j < tab.cas.GetLength(0); j++)
			{
				for (int i = 0; i < tab.cas.GetLength(1); i++)
				{
					switch (tab.cas[j, i].estado)
					{

					case ('o'):
						if (tab.cas[j, i].mina)
						{
							salida.Write("M");
						}
						else
							salida.Write("B");
						break;

					case ('x'):
						if (tab.cas[j, i].mina)
						{
							salida.Write("N");
						}
						else
							salida.Write("A");
						break;

					case ('0'):
						salida.Write("C");
						break;

					case ('1'):
						salida.Write("D");
						break;

					case ('2'):
						salida.Write("E");
						break;

					case ('3'):
						salida.Write("F");
						break;

					case ('4'):
						salida.Write("G");
						break;

					case ('5'):
						salida.Write("H");
						break;

					case ('6'):
						salida.Write("I");
						break;

					case ('7'):
						salida.Write("J");
						break;

					case ('8'):
						salida.Write("K");
						break;


					default:
						salida.Write("?");
						break;
					}
				}

				salida.WriteLine();
			}

			salida.Close();
		}

		public static Tablero Cargar()
		{
			Tablero tab;
			StreamReader partida;

			partida = new StreamReader("guardado.txt");

			//Lee la primera linea del archivo "Archivo de Guardado"
			partida.ReadLine();

			// Pasamos numMinas como 0 para evitar que coloque nuevas minas en posiciones aleatorias.
			tab = creaTablero(int.Parse(partida.ReadLine()), int.Parse(partida.ReadLine()), 0);


			for (int j = 0; j < tab.cas.GetLength(0); j++)
			{
				for (int i = 0; i < tab.cas.GetLength(1); i++)
				{
					switch (partida.Read())
					{

					case ('B'):
						tab.cas[j, i].estado = 'o';
						break;

					case ('M'):
						tab.cas[j, i].estado = 'o';
						tab.cas[j, i].mina = true;
						break;

					case ('N'):
						tab.cas[j, i].estado = 'x';
						tab.cas[j, i].mina = true;
						break;

					case ('A'):
						tab.cas[j, i].estado = 'x';
						break;

					case ('C'):
						tab.cas[j, i].estado = '0';
						break;

					case ('D'):
						tab.cas[j, i].estado = '1';
						break;

					case ('E'):
						tab.cas[j, i].estado = '2';
						break;

					case ('F'):
						tab.cas[j, i].estado = '3';
						break;

					case ('G'):
						tab.cas[j, i].estado = '4';
						break;

					case ('H'):
						tab.cas[j, i].estado = '5';
						break;

					case ('I'):
						tab.cas[j, i].estado = '6';
						break;

					case ('J'):
						tab.cas[j, i].estado = '7';
						break;

					case ('K'):
						tab.cas[j, i].estado = '8';
						break;


					default:
						tab.cas[j, i].estado = '?';
						break;
					}
				}

				partida.ReadLine();
			}

			partida.Close();

			return tab;

		}

		static void Main()
		{
			Jugar();
		}
	}
}
