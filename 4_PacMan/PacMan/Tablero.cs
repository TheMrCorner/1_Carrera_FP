using System;
using System.IO;

namespace ConsoleApplication2
{
    public class Tablero
    {
        //Dimensiones del tablero
        int fils, cols;

        //Contenido de las casillas
        enum Casilla { Blanco, Muro, Comida, Vitamina, MuroCelda };

        //Matriz de casillas (tablero)
        Casilla[,] cas;

        //Reperesentación de los personajes
        struct Personaje
        {
            public int posX, posY;  //Posición actual
            public int dirX, dirY;  //Dirección de mov
            public int defX, defY;  //Posición inicial
        }

        //Vector de personajes (0 pacman, el resto fantasmas)
        Personaje [] pers;

        //Array de los colores de los personajes
        ConsoleColor[] colors = {ConsoleColor.DarkYellow, ConsoleColor.Red, 
			ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.DarkBlue};

        int lapFantasma;   //Tiempo de retardo de los fantasmas
        int numComida;    //numero de casillas restantes con comida
        int numNivel;    //Nivel del juego

        Random rnd; //Generador de random 

        //Bool para depuración en consola
        private bool debug = true;


        public Tablero(string file)
		{
			//Inicialización del generador aleatorio
			if (!debug)
				rnd = new Random ();
			else   //Inicialización en debug
				rnd = new Random (100);

            //Cogemos las dimensiones que tendrá el tablero
            getDims(file);

            cas = new Casilla[fils, cols]; //Creamos el tablero

            //Inicializamos el array de personajes
            pers = new Personaje[5];

			StreamReader leeNivel = new StreamReader (file);

            //Rellenamos el tablero con la información necesaria
			for (int j = 0; j < fils; j++) {
				string lectura = leeNivel.ReadLine ();  //Leemos una línea del archivo

                lectura = lectura.Replace(" ", string.Empty);  //Sustituímos todos los espacios por valor nulo

				for (int i = 0; i < lectura.Length; i++)  //Vemos las columnas
				{
                    int h = 0;  //Variable que controla las columnas reales (cols)

                    char posCas = lectura[i];
                        switch (posCas)  //...damos la información
                        {
                            case '0':
                                cas[j, i] = Casilla.Blanco;
                                break;
                            case '1':
                                cas[j, i] = Casilla.Muro;
                                break;
                            case '2':
                                cas[j, i] = Casilla.Comida;
                                break;
                            case '3':
                                cas[j, i] = Casilla.Vitamina;
                                break;
                            case '4':
                                cas[j, i] = Casilla.MuroCelda;
                                break;
                            case '5':  //Info del fantasma 1
                                cas[j, i] = Casilla.Blanco;
                                pers[1].defX = h;
                                pers[1].defY = j;
                                pers[1].posX = h;
                                pers[1].posY = j;
                                pers[1].dirX = 0;
                                pers[1].dirY = 0;
                                break;
                            case '6':  //Info del fantasma 2
                                cas[j, i] = Casilla.Blanco;
                                pers[2].defX = h;
                                pers[2].defY = j;
                                pers[2].posX = h;
                                pers[2].posY = j;
                                pers[2].dirX = 0;
                                pers[2].dirY = 0;
                                break;
                            case '7':  //Info del fantasma 3
                                cas[j, i] = Casilla.Blanco;
                                pers[3].defX = h;
                                pers[3].defY = j;
                                pers[3].posX = h;
                                pers[3].posY = j;
                                pers[3].dirX = 0;
                                pers[3].dirY = 0;
                                break;
                            case '8':  //info del fantasma 4
                                cas[j, i] = Casilla.Blanco;
                                pers[4].defX = h;
                                pers[4].defY = j;
                                pers[4].posX = h;
                                pers[4].posY = j;
                                pers[4].dirX = 0;
                                pers[4].dirY = 0;
                                break;
                            case '9':  //Info de pacman
                                cas[j, i] = Casilla.Blanco;
                                pers[0].defX = h;
                                pers[0].defY = j;
                                pers[0].posX = h;
                                pers[0].posY = j;
                                pers[0].dirX = 0;
                                pers[0].dirY = 0;
                                break;
                        }
				}
			}

			leeNivel.Close ();
		}

        //Obtenemos las dimensiones del tablero
        public void getDims(string file)
        {
            bool filas = true;

            StreamReader leeNivel = new StreamReader(file);

            while (!leeNivel.EndOfStream)
            {
                string lect = leeNivel.ReadLine();

                lect = lect.Replace(" ", string.Empty);

                if (lect.Length > 5){
                    cols = lect.Length;
                    fils++;
                }
            }

            Console.WriteLine(fils);
            Console.WriteLine(cols);

            Console.ReadLine();

            Console.Clear();

            leeNivel.Close();
        }

        public void Dibuja()
        {
            Console.WriteLine(fils);
            Console.WriteLine(cols);

            for (int j = 0; j < fils; j++)
            {
                for (int i = 0; i < cols; i++)
                {
                    if (j == pers[0].posX && i == pers[0].posY)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write("C");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (j == pers[1].posX && i == pers[1].posY)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write("1");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (j == pers[2].posX && i == pers[2].posY)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write("2");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (j == pers[3].posX && i == pers[3].posY)
                    {
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.Write("3");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (j == pers[4].posX && i == pers[4].posY)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write("4");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (cas[j, i] == Casilla.Blanco)
                    {
                        Console.Write(" ");
                    }
                    else if (cas[j, i] == Casilla.Comida)
                    {
                        Console.Write(".");
                    }
                    else if (cas[j, i] == Casilla.Muro)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (cas[j, i] == Casilla.MuroCelda)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write(" ");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (cas[j, i] == Casilla.Vitamina)
                    {
                        Console.Write("*");
                    }
                }
                Console.WriteLine();
            }

            if (!debug)
            {
                Console.WriteLine("No tamos en debug.");
            }
            else  //Información en pantalla si debug está activado
            {
                for (int h = 0; h < pers.Length; h++)
                {
                    Console.WriteLine("Mov pers {0} {1} {2} ", h, pers[h].posX, pers[h].posY);
                }
            }
            Console.ReadLine();
        }
    }
}
