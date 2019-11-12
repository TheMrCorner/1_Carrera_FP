using System;

namespace ConsoleApplication2
{
    class ListaPares
    {
        class Nodo
        {
            public int x, y;
            public Nodo sig; // enlace al siguiente nodo

            // constructor 
            public Nodo(int i, int j) { x = i; y = j; }
        }

        // atributos de la lista enlazada: referencia al primero y al último		
        Nodo pri, ult, act;

        // constructora de listas
        public ListaPares() { act = null; }

        // obtener primer elto de la lista
        public void primero(out int i, out int j)
        {
            if (pri == null) throw new Exception("Error primero: lista vacia");
            i = pri.x; j = pri.y;
        }

        // obtener ultimo elto de la lista
        public void ultimo(out int i, out int j)
        {
            if (ult == null) throw new Exception("Error ultimo: lista vacia");
            i = ult.x; j = ult.y;
        }

        // ver si un elto esta en la lista
        public bool esta(int i, int j)
        {
            // iniciamos con aux al ppio
            Nodo aux = pri;

            // busqueda: avanzazmos con aux mientras no llegemoa al final y no encontremos elto
            while (aux != null && (aux.x != i || aux.y != j)) aux = aux.sig;

            // si no hemos llegado el final, es pq el elto está en la lista
            return (aux != null);
        }

        // insertar elto al ppio de la lista
        public void insertaIni(int x, int y)
        {
            // si la lista es vacia creamos nodo y apuntamos a el pri y ult
            if (pri == null)
            {
                pri = new Nodo(x, y);
                pri.sig = null;
                ult = pri;
            }
            else
            { // si no es vacia creamos nodo y lo enganchamos al ppio
                Nodo aux = new Nodo(x, y);
                aux.sig = pri;
                pri = aux;
            }
        }


        // insertar elto al final de la lista
        public void insertaFin(int x, int y)
        {
            // si es vacia creamos nodo y apuntamos a el ppi y ult
            if (pri == null)
            {
                pri = new Nodo(x, y);
                pri.sig = null;
                ult = pri;
            }
            else
            { // si no, creamos nodo apuntado por ult.sig y enlazamos
                ult.sig = new Nodo(x, y);
                ult = ult.sig;
                ult.sig = null;
            }
        }



        // elimina elto dado de la lista, si esta
        public bool eliminaElto(int x, int y)
        {
            // lista vacia
            if (pri == null) return false;// throw new Exception("Error eliminaElto: lista vacia");
            else
            {
                // eliminar el primero
                if (x == pri.x && y == pri.y)
                {
                    // si solo tiene un elto
                    if (pri == ult)
                        pri = ult = null;
                    // si tiene más de uno
                    else
                        pri = pri.sig;
                    return true;
                }
                // eliminar otro distinto al primero
                else
                {
                    // busqueda. aux al ppio
                    Nodo aux = pri;
                    // recorremos lista buscando el ANTERIOR al que hay que eliminar (para poder luego enlazar)
                    while (aux.sig != null && (x != aux.sig.x || y != aux.sig.y))
                        aux = aux.sig;

                    // si lo encontramos
                    if (aux.sig != null)
                    {
                        // si es el ultimo cambiamos referencia al ultimo
                        if (aux.sig == ult)
                            ult = aux;
                        // puenteamos
                        aux.sig = aux.sig.sig;
                        return true;
                    }
                    else return false;
                }
            }
        }

        // elimina primer elto de la lista
        public void eliminaIni()
        {
            if (pri == null) throw new Exception("Error eliminaIni: lista vacia");
            if (pri == ult) pri = ult = null;
            else pri = pri.sig;
        }


        public void nEsimo(int n, out int x, out int y)
        {
            Nodo aux = pri;
            int i = 0;
            while (i < n && aux != null) { aux = aux.sig; i++; }
            if (aux == null) throw new Exception("Error nEsimo: lista con menos de " + n + " elementos");
            else { x = aux.x; y = aux.y; }
        }


        // inicizlización del iterador. Lo colocamos al ppio
        public void iniciaRecorrido()
        {
            act = pri;
        }

        public bool dame_actual_y_avanza(out int x, out int y)
        {
            x = y = 0;
            // si estamos al final, ya no hay actual y devolvemos false
            if (act == null)
                return false;
            else
            { // si no, info del nodo, avanzamos act y devolvemos true
                x = act.x;
                y = act.y;
                act = act.sig;
                return true;
            }
        }

        // auxiliar solo para depuración
        public void verLista()
        {
            Console.Write("Lista: ");
            Nodo aux = pri;
            while (aux != null)
            {
                Console.Write("(" + aux.x + "," + aux.y + ") ");
                aux = aux.sig;
            }
            Console.Write("\n\n");
        }
    }
}
