using System;

namespace Proyecto1marcianitos
{
	class MainClass
	{
		static void Main (string[] args)
		{
			Random rnd = new Random (); 

			//Variables constantes del juego
			int ancho = 60, alto = 25;
			int delta = 0;
			bool deltaVal = false, player = true;

			//Variables de jugador y del enemigo
			int playerX = 0, playerY = (alto - 1);
			int marsX = 7, marsY = 3;
			int bombaX = -1, bombaY = 0;
			int balaX = -1, balaY = 0;

			//Solicitud del nivel de dificultad
			while (deltaVal == false) {

				Console.Write ("Elige el nivel de dificultad:\n 50 difícil \n 200 fácil \n ");
				delta = int.Parse (Console.ReadLine ());

				if (delta <= 200 && delta >= 50) {
					deltaVal = true;
				} 

			}

			Console.Clear ();

			//Juego
			while (player == true){

				//controles o input del jugador
				if (Console.KeyAvailable) {
					string dir = (Console.ReadKey (true)).KeyChar.ToString ();
					if (dir == "a" && playerX > 0) {
						playerX--;
					} 
					else if (dir == "d" && playerX < (ancho - 1)) {
						playerX++;
					}
					else if (dir == "l" && balaY <= 0) {
						balaX = playerX;
						balaY = playerY;
					}
					while (Console.KeyAvailable)
						Console.ReadKey (true);
				}

				//Lógica del juego: marciano en X
				if (marsX < (ancho - 2) && marsX > 0) {
					marsX = marsX + rnd.Next (-1, 2);
				} 
				else if (marsX <= 0) {
					marsX++;
				} 
				else if (marsX >= (ancho - 2)) {
					marsX--;
				}
				//Lógica del juego: marciano en Y
				if (marsY < (alto / 2) && marsY > 0) {
					marsY = marsY + rnd.Next (-1, 2);
				} 
				else if (marsY <= 0) {
					marsY++;
				} 
				else if (marsY >= (alto / 2)) {
					marsY--;
				}


				//Lógica del juego: bomba 
				if (bombaX == -1) {
					bombaX = marsX + 1;
					bombaY = marsY + 1;
				} 
				else if (bombaY >= alto)
					bombaX = -1;
				else if (bombaY < alto) {
					bombaY++;
				}

				//Lógica de la bala
				if (balaY > 0) {
					balaY--;
				}
				else if (balaY <= 0) {
					balaX = -1;
				}


				//Lógica del juego: colisiones
				if ((balaY == marsY || balaY == marsY - 1) && (balaX == marsX || balaX == marsX + 1 || balaX == marsX + 2)) //Colisión de la bala del jugador con el marciano
					player = false;
				else if (bombaX == playerX && bombaY == playerY) //Colisión de la bomba del marciano con el jugador
					player = false;
				else if (balaX == bombaX && balaY == bombaY) { //Colisión de la bala con la bomba
					balaX = -1;
					balaY = 0;
					bombaX = -1;
				}


				//Renderizado Gráfico del juego
				Console.Clear ();
				for (int i = 0; i < alto; i++) {
					Console.SetCursorPosition (ancho, i);
					Console.WriteLine ("|");
				}
				Console.SetCursorPosition (marsX, marsY);
				Console.Write ("***");
				Console.SetCursorPosition (playerX, playerY);
				Console.Write ("@");
				if (balaX >= 0) {
					Console.SetCursorPosition (balaX, balaY);
					Console.Write ("^");
				}
				if (bombaX >= 0) {
					Console.SetCursorPosition (bombaX, bombaY);
					Console.Write ("=");
				}
				Console.SetCursorPosition (ancho, alto);

				System.Threading.Thread.Sleep(delta);
			}

			//Fin del juego
			Console.Clear();
			Console.WriteLine("Game Over");
		}

	}
}