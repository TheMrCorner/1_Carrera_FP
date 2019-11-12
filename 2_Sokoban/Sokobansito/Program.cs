using System;
using System.IO;

//Juego del sokoban, práctica 2 Fundamentos de Programación

namespace SOKOBAN
{
	class MainClass
	{
		public enum Casilla {Muro, Libre, Destino};

		public struct Tablero{
			public int fils, cols;     //Filas y columnas del nivel
			public Casilla [,] fijas;  //Casillas fijas: Muro, libre, destino
			public bool [,] cajas;     //Posición de las cajas
			public int jugX, jugY;     //Posición del jugador
		};

		public static void Main ()
		{
			string archivo = "levels.txt";
			int nivel = 0;
			int movimientos = 0;
			Tablero tab;
			bool win = false;
			bool mover = false;
			char dir = 'a';

			Console.Write ("Elige el nivel: ");
			nivel = int.Parse (Console.ReadLine ());

			while ((nivel < -1) || (nivel > 50)) {
				Console.WriteLine ("Nivel inexsistente, prueba otra vez: ");
				nivel = int.Parse (Console.ReadLine ());

			}

			tab = LeeNivel (archivo, nivel);

			Dibuja (tab, movimientos);

			//Bucle principal del juego
			while (win != true) {
				//Llamamos a LeeEntrada una vez por frame
				dir = LeeEntrada ();

				//Comprobamos si se produce movimiento
				mover = Siguiente (tab.jugX, tab.jugY, dir, tab, 0, 0);

				//Si se produce movimiento, actualizamos el tablero y lo dibujamos
				if (mover == true) {
					//Movemos las cajas y el jugador hacia la posición indicada
					Mueve (ref tab, dir);
					//+1 contador de movimientos
					movimientos++;
					//Dibujamos
					Dibuja (tab, movimientos);
					//Comprobamos que todas las cajas están en Destino
					win = Termina (tab);
				}
			}

			Console.WriteLine ("¡Nivel Completado!");
		}

		//Inicializa la matriz del tablero
		public static void CreaTablero (out Tablero tab){

			//Creamos los arrays
			tab.fijas = new Casilla [50,50];
			tab.cajas = new bool [50,50];

			//Inicializamos el array Casilla con el valor Muro
			for (int i = 0; i < 50; i++)
				for (int j = 0; j < 50; j++)
					tab.fijas [i,j] = Casilla.Muro;

			//Inicializamos el array de cajas a false
			for (int g = 0; g < 50; g++)
				for (int h = 0; h < 50; h++)
					tab.cajas [g,h] = false;

			//Inicializamos el resto de valores
			tab.fils = tab.cols = 0;
			tab.jugX = tab.jugY = 0;
		}

		//Leemos el nivel que vamos a jugar
		public static Tablero LeeNivel (string file, int nivel){
			string fila;  //guarda la línea actual de lectura
			char c;       //La usaremos para rellenar el tablero
			int i = 0;    //Filas
			int j;        //Columnas
			int max = 0;      //Número máximo de columnas
			string level = null;
			bool finNivel = false;
			Tablero tab;


			//Creamos el tablero vacío
			CreaTablero (out tab);

			//Iniciamos el flujo de lectura
			StreamReader nivelArchivo;

			nivelArchivo = new StreamReader (file);

			//Leemos el archivo en busca del nivel
			while (level != ("Level " + nivel)){
				level = nivelArchivo.ReadLine();
			}

			//Rellenamos el tablero con la posición de las distintas casillas
			while ((finNivel == false) && !nivelArchivo.EndOfStream){
				//guardamos el valor de la línea que estamos leyendo
				fila = nivelArchivo.ReadLine();
				//Comprobamos que no es el final del nivel
				if (fila.Length <= 0)
					finNivel = true;
				//Letra por letra comprobamos qué tipo de casilla es
				for (j = 0; j < fila.Length; j++){
					c = fila [j];
					switch (c) {
					//Muro(s)
					case '#':
						tab.fijas [i, j] = Casilla.Muro;
						break;

						//Casilla libre
					case ' ':
						tab.fijas [i,j] = Casilla.Libre;
						break;

						//Casilla de destino de la caja
					case '.':
						tab.fijas [i,j] = Casilla.Destino;
						break;

						//Casilla con caja
					case '$':
						tab.cajas [i, j] = true;
						tab.fijas [i, j] = Casilla.Libre;
						break;

						//Casilla con jugador
					case '@':
						tab.jugX = i;
						tab.jugY = j;
						break;
						//Casilla con caja en casilla destino
					case '*':
						tab.fijas [i, j] = Casilla.Destino;
						tab.cajas [i, j] = true;
						break;
					case '+':
						tab.jugX = i;
						tab.jugY = j;
						tab.fijas [i, j] = Casilla.Destino;
						break;
					}
				}

				//**Calculamos el número de columnas total**
				//La primera vez max está a 0
				//por lo que le damos el valor de la primera línea
				if(max <= 0)
					max = j;
				//Si ese valor es menor que j
				//Le asignamos el valor actual de j
				else if (max < j)
					max = j;

				i++;
			}

			tab.fils = i;
			tab.cols = max;

			nivelArchivo.Close();

			return tab;
		}

		//Dibujamos el escenario
		public static void Dibuja (Tablero tab, int mov){
			int i = 0;

			//Borramos la consola cada vez que actualizamos el juego
			Console.Clear();

			//Dibujamos el nivel con el estado actual
			while (i < tab.fils){
				for(int j = 0; j < tab.cols; j++){
					//Primero comprobamos que no estamos pintando al jugador
					//Si es así, pintamos el avatar
					if (i == tab.jugX && j == tab.jugY) {
						Console.BackgroundColor = ConsoleColor.Green;
						Console.Write ("ºº");
						Console.BackgroundColor = ConsoleColor.Black;
					} 
					//Si la posición en la que estamos corresponde a una caja
					//Comprobamos primero que NO está en una casilla de destino, o cualquier fija
					//Después la dibujamos con el color correspondiente
					else if (tab.cajas [i, j]) {
						if (tab.fijas [i, j] != Casilla.Destino) {
							Console.BackgroundColor = ConsoleColor.Red;
							Console.Write ("[]");
							Console.BackgroundColor = ConsoleColor.Black;
						} 
						else {
							Console.BackgroundColor = ConsoleColor.Yellow;
							Console.Write ("[]");
							Console.BackgroundColor = ConsoleColor.Black;
						}
					}
					//Dibujamos los muros pintando el color de fondo de la consola de azul oscuro
					else if (tab.fijas [i, j] == Casilla.Muro) {
						Console.BackgroundColor = ConsoleColor.DarkBlue;
						Console.Write ("  ");
						Console.BackgroundColor = ConsoleColor.Black;
					}
					//Pintamos los muros de cián oscuro
					else if (tab.fijas [i, j] == Casilla.Libre) {
						Console.BackgroundColor = ConsoleColor.DarkCyan;
						Console.Write ("  ");
						Console.BackgroundColor = ConsoleColor.Black;
					} 
					//Pintamos las casillas de destino
					//Si hay una caja sobre ellas, no la pintamos y pintamos la caja
					else if ((tab.fijas [i, j] == Casilla.Destino) && (!tab.cajas [i,j])) {
						Console.BackgroundColor = ConsoleColor.DarkCyan;
						Console.Write ("()");
						Console.BackgroundColor = ConsoleColor.Black;
					}
				}
				Console.WriteLine();
				i++;
			}


			Console.WriteLine ();
			Console.WriteLine ("Movimientos ejecutados: " + mov);
		}

		//Leemos la entrada de teclado
		public static char LeeEntrada(){
			char h = 'j';

			//Si detecta una entrada de teclado
			if (Console.KeyAvailable){
				//Guardamos el valor de la tecla en un string
				string tecla = Console.ReadKey ().Key.ToString();

				//Comprobamos la dirección con el valor del string tecla
				//Asginamos a h la dirección
				switch (tecla){
				//Movemos arriba
				case ("UpArrow"):
					h = 'u';
					break;
					//Movemos abajo
				case ("DownArrow"):
					h = 'd';
					break;
					//Movemos izquierda
				case ("LeftArrow"):
					h = 'l';
					break;
					//Movemos derecha
				case ("RightArrow"):
					h = 'r';
					break;
				}
			}

			//Devolvemos la dirección
			return h;
		}

		//Determina si podemos movernos o no
		public static bool Siguiente (int x, int y, char dir, Tablero tab, int nextX, int nextY){
			bool mover = false;  //Determina si podemos o no avanzar, se inicializa a false

			nextX = x;
			nextY = y;

			//Variables para calcular la casilla siguiente a la que estamos comprobando
			int temp_x = x;
			int temp_y = y;

			//Calculamos la siguiente casilla a la que nos movemos
			switch (dir){
			case ('u'):
				nextX--;
				temp_x -= 2;
				break;
			case ('d'):
				nextX++;
				temp_x += 2;
				break;
			case ('l'):
				nextY--;
				temp_y -= 2;
				break;
			case ('r'):
				nextY++;
				temp_y += 2;
				break;
			} 

			//Comprobamos qué hay en la casilla siguiente
			if (dir != 'j') {
				//Si hay un muro no avanzamos
				if (tab.fijas [nextX, nextY] == Casilla.Muro)
					mover = false;
				//Si hay caja...
				else if (tab.cajas [nextX, nextY] == true){
					//... y no hay otra caja delante o un muro, avanzamos
					if (!tab.cajas [temp_x, temp_y] && (tab.fijas [temp_x, temp_y] != Casilla.Muro))
						mover = true;
				}
				//Si es un espacio vacío, avanzamos 
				else
					mover = true;
			}

			return mover;
		}

		public static void Mueve (ref Tablero tab, char dir){
			int i = tab.jugX;
			int j = tab.jugY;
			int cajX = 0;
			int cajY = 0;

			//Guardamos las coordenadas en función del valor de dir
			switch (dir){
			case ('u'):
				i--;
				break;
			case ('d'):
				i++;
				break;
			case ('l'):
				j--;
				break;
			case ('r'):
				j++;
				break;
			}

			//Si hay una caja, la movemos
			if (tab.cajas [i, j] == true) {
				//Ponemos la casilla actual de la caja a false
				cajX = i;
				cajY = j;
				//La movemos en la dir correspondiente
				if (dir == 'u') {
					cajX--;
				} 
				else if (dir == 'd') {
					cajX++;
				} 
				else if (dir == 'l') {
					cajY--;
				} 
				else if (dir == 'r') {
					cajY++;
				}

				tab.cajas [cajX, cajY] = true;
				tab.cajas [i, j] = false;
			}
			if(tab.fijas [tab.jugX, tab.jugY] != Casilla.Destino)
				tab.fijas [tab.jugX, tab.jugY] = Casilla.Libre;
			//Después movemos al jugador a la posición indicada
			tab.jugX = i;
			tab.jugY = j;


		}

		//Comprobamos que hemos completado el juego
		public static bool Termina (Tablero tab){
			int i = 0;
			int j;
			bool termina = true;

			//Realizamos una búsqueda en el Tablero para comprobar si las casillas Destino están vacias
			//o si hay una caja en ellas
			while ((termina == true) && (i < tab.fils)) {
				j = 0;
				while ((termina == true) && (j < tab.cols)) {
					//Si la casilla que comprobamos es Destino
					//Y esa misma casilla en cajas es cierta (true)
					//Ponemos termina a false
					if ((tab.fijas [i, j] == Casilla.Destino) && (!tab.cajas [i, j]))
						termina = false;
					//Si no, seguimos buscando
					else
						j++;
				}
				i++;
			}

			return termina;
		}
	}
}