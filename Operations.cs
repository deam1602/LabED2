using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using static System.Collections.Specialized.BitVector32;
using Laboratorio_ED2_1;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace Laboratorio_ED2_1
{
    public class Operations
    {
        public static AVL<Persona> arbol = new AVL<Persona>();
        public static List<string> Lcomprimidos = new List<string>();
        public static List<string> Ldescomprimidos = new List<string>();
        public static List<string> dic = new List<string>();//diccionario

        public static void leerArchivo()
        {
            
           string filePath = "C:/Users/Diego/OneDrive/Documentos/Diego/Universidad/5. Cuarto Ciclo/Estructuras II/Laboratorio_ED2_1/input (2).csv"; // Ruta archivo
            string filePathJsonL = "C:/Users/Diego/OneDrive/Documentos/Diego/Universidad/5. Cuarto Ciclo/Estructuras II/Laboratorio_ED2_1/datosConvertidos.txt";
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 2)
                    {
                        string action = parts[0].Trim();
                        string dataJson = parts[1].Trim();

                        Persona person = Newtonsoft.Json.JsonConvert.DeserializeObject<Persona>(dataJson);
                        commandReader(action, person, arbol);
                        
                    }
                }
                GuardarArbolEnJsonl(arbol, filePathJsonL);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo: " + ex.Message);
            }
        }

        public static void BuscarArchivoCifrar(long dpi)
        {
            string carpeta = "C:/Users/Diego/OneDrive/Documentos/Diego/Universidad/5. Cuarto Ciclo/Estructuras II/Laboratorio_ED2_1/inputs"; // Cambia esto por la ruta de tu carpeta
            long buscado = dpi; // Cambia esto por el número entero que quieres verificar 
            string llave = "cifrado";
            // Obtener los nombres de los archivos en la carpeta
            string[] nombresArchivos = Directory.GetFiles(carpeta);

            // Iterar sobre cada nombre de archivo
            foreach (string nombreArchivo in nombresArchivos)
            {
                // Obtener solo el nombre del archivo sin la ruta ni la extensión
                string nombreSinExtension = Path.GetFileNameWithoutExtension(nombreArchivo);
                string[] dpiEnNombre = nombreSinExtension.Split('-');
                long dpiInt = Convert.ToInt64(dpiEnNombre[1]);

                // Verificar si el nombre del archivo es igual al dpi buscado
                if (dpiInt == buscado)
                {
                    try
                    {
                        // Leer el contenido del archivo y guardarlo en un string
                        string contenido = File.ReadAllText(nombreArchivo);
                        Console.WriteLine($"\n\nCarta {nombreSinExtension} cifrada:\n");
                        Cifrado(nombreSinExtension, contenido, llave, '-');


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al leer el archivo {nombreArchivo}: {ex.Message}");
                    }
                }
            }

        }
        public static void BuscarArchivoDescifrar(long dpi)
        {
            string carpeta = "C:/Users/Diego/OneDrive/Documentos/Diego/Universidad/5. Cuarto Ciclo/Estructuras II/Laboratorio_ED2_1/Cartas cifradas"; // Cambia esto por la ruta de tu carpeta
            long buscado = dpi; // Cambia esto por el número entero que quieres verificar 
            string llave = "cifrado";
            // Obtener los nombres de los archivos en la carpeta
            string[] nombresArchivos = Directory.GetFiles(carpeta);

            // Iterar sobre cada nombre de archivo
            foreach (string nombreArchivo in nombresArchivos)
            {
                // Obtener solo el nombre del archivo sin la ruta ni la extensión
                string nombreSinExtension = Path.GetFileNameWithoutExtension(nombreArchivo);
                string[] dpiEnNombre = nombreSinExtension.Split('-');
                long dpiInt = Convert.ToInt64(dpiEnNombre[1]);

                // Verificar si el nombre del archivo es igual al dpi buscado
                if (dpiInt == buscado)
                {
                    try
                    {
                        // Leer el contenido del archivo y guardarlo en un string
                        string contenido = File.ReadAllText(nombreArchivo);
                        Console.WriteLine($"\n\nCarta {nombreSinExtension} cifrada:\n");
                        Descifrado(contenido, llave);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al leer el archivo {nombreArchivo}: {ex.Message}");
                    }
                }
            }

        }

        public static void Cifrado(string nombreArchivo, string entrada, string llave, char padChar)
        {
            entrada = (entrada.Length % llave.Length == 0) ? entrada : entrada.PadRight(entrada.Length - (entrada.Length % llave.Length) + llave.Length, padChar);
            StringBuilder salida = new StringBuilder();
            int totalCaracteres = entrada.Length;
            int totalColumnas = llave.Length;
            int totalFilas = (int)Math.Ceiling((double)totalCaracteres / totalColumnas);
            char[,] filaCaracteres = new char[totalFilas, totalColumnas];
            char[,] colCaracteres = new char[totalColumnas, totalFilas];
            char[,] colOrdenadas = new char[totalColumnas, totalFilas];
            int filaActual, columnaActual, i, j;
            int[] mezclarIndexes = ObtenerIndex(llave);

            for (i = 0; i < totalCaracteres; ++i)
            {
                filaActual = i / totalColumnas;
                columnaActual = i % totalColumnas;
                filaCaracteres[filaActual, columnaActual] = entrada[i];
            }

            for (i = 0; i < totalFilas; ++i)
                for (j = 0; j < totalColumnas; ++j)
                    colCaracteres[j, i] = filaCaracteres[i, j];

            for (i = 0; i < totalColumnas; ++i)
                for (j = 0; j < totalFilas; ++j)
                    colOrdenadas[mezclarIndexes[i], j] = colCaracteres[i, j];

            for (i = 0; i < totalCaracteres; ++i)
            {
                filaActual = i / totalFilas;
                columnaActual = i % totalFilas;
                salida.Append(colOrdenadas[filaActual, columnaActual]);
            }
            EscribirCifrado(nombreArchivo, salida.ToString());
            Console.WriteLine(salida.ToString());
        }

        public static void Descifrado(string entrada, string llave)
        {
            StringBuilder salida = new StringBuilder();
            int totalCaracteres = entrada.Length;
            int totalColumnas = (int)Math.Ceiling((double)totalCaracteres / llave.Length);
            int totalFilas = llave.Length;
            char[,] filaCaracteres = new char[totalFilas, totalColumnas];
            char[,] colCaracteres = new char[totalColumnas, totalFilas];
            char[,] colDesordenadas = new char[totalColumnas, totalFilas];
            int filaActual, columnaActual, i, j;
            int[] mezclarIndexes = ObtenerIndex(llave);

            for (i = 0; i < totalCaracteres; ++i)
            {
                filaActual = i / totalColumnas;
                columnaActual = i % totalColumnas;
                filaCaracteres[filaActual, columnaActual] = entrada[i];
            }

            for (i = 0; i < totalFilas; ++i)
                for (j = 0; j < totalColumnas; ++j)
                    colCaracteres[j, i] = filaCaracteres[i, j];

            for (i = 0; i < totalColumnas; ++i)
                for (j = 0; j < totalFilas; ++j)
                    colDesordenadas[i, j] = colCaracteres[i, mezclarIndexes[j]];

            for (i = 0; i < totalCaracteres; ++i)
            {
                filaActual = i / totalFilas;
                columnaActual = i % totalFilas;
                salida.Append(colDesordenadas[filaActual, columnaActual]);
            }

            Console.WriteLine(salida.ToString());
        }

        public static void EscribirCifrado(string nombreArchivo, string texto)
        {
            string contenido = texto;

            string rutaArchivo = $"C:/Users/Diego/OneDrive/Documentos/Diego/Universidad/5. Cuarto Ciclo/Estructuras II/Laboratorio_ED2_1/Cartas cifradas/{nombreArchivo}-cifrado";

            try
            {
                // Crear un StreamWriter y escribir el contenido en el archivo
                using (StreamWriter writer = new StreamWriter(rutaArchivo))
                {
                    writer.Write(contenido);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al escribir en el archivo: {ex.Message}");
            }
        }

        private static int[] ObtenerIndex(string llave)
        {
            int tamaniollave = llave.Length;
            int[] indexes = new int[tamaniollave];
            List<KeyValuePair<int, char>> llaveOrdenada = new List<KeyValuePair<int, char>>();
            int i;

            for (i = 0; i < tamaniollave; ++i)
                llaveOrdenada.Add(new KeyValuePair<int, char>(i, llave[i]));

            llaveOrdenada.Sort(
                delegate (KeyValuePair<int, char> par1, KeyValuePair<int, char> pair2) {
                    return par1.Value.CompareTo(pair2.Value);
                }
            );

            for (i = 0; i < tamaniollave; ++i)
                indexes[llaveOrdenada[i].Key] = i;

            return indexes;
        }

        public static string MostrarCompresion(long dpiABuscar)
        {
            string buscarDPI = Convert.ToString(dpiABuscar);  
            string encontrado = Lcomprimidos.Find(s  => s.Contains(buscarDPI));
            return encontrado;
        }

        public static string MostrarDescompresion(long dpiABuscar)
        {
            string buscarDPI = Convert.ToString(dpiABuscar);
            string encontrado = Ldescomprimidos.Find(s => s.Contains(buscarDPI));
            return encontrado;
        }

        public static string decompression(string comprimido)
        {
            string texto = "";
            string siguiente = "";
            int puntero = 0;
            texto = comprimido; //se iguala al texto a descomprimir
            string[] ArregloComprimido = comprimido.Split(); //se ingresa el string comprimido a un arreglo
            comprimido = "";
            for (int i = 0; i < texto.Length; i += 2)
            {

                if (ArregloComprimido[i].Length == 0)
                    break;
                // se obtiene el puntero
                puntero = int.Parse(ArregloComprimido[i]);

                // se obtiene el caracter
                siguiente = ArregloComprimido[i + 1];
                if (siguiente == "")
                {
                    siguiente = "_";
                }
                if (siguiente != "null")
                    comprimido += dic[puntero] + siguiente;
                else
                    comprimido += dic[puntero];

                puntero = 0;
                siguiente = "";

            }
            puntero = 0;
            siguiente = "";
            dic.Clear();
            return comprimido;
        }

        public static string compression(string compresion)//método de compresión LZ78
        {

            string texto = "";
            string Comparartxt = "";//string para comparar textos
            int index = 0, retrn = 0 /*toma otro caracter y lo compara con el diccionario*/;
            texto = compresion; //se igualan las compañias
            compresion = "0 " + texto[0] + "\n"; // primer caracter
            dic.Add(""); // primer elemento del diccionario es null
            dic.Add(texto[0] + "");

            for (int indexTexto = 1; indexTexto < texto.Length; indexTexto++)
            {

                Comparartxt += texto[indexTexto]; //Se guarda la letra

                if (dic.IndexOf(Comparartxt) != -1)
                {
                    index = dic.IndexOf(Comparartxt);

                    retrn = 1; //al validar que se repite la letra, se crea la condición para que entre en el if en la siguiente letra
                    if (indexTexto + 1 == texto.Length) //end of line
                        compresion += index + " null\n";

                }
                else
                {
                    if (retrn == 1)// if de repetidos
                        compresion += index + " " + Comparartxt[Comparartxt.Length - 1] + "\n"; //al entrar coloca el index de la letra repetida y le agrega la letra actual

                    else
                        compresion += "0 " + Comparartxt + "\n"; //si la letra no se ha repetido se coloca "0,letra"

                    dic.Add(Comparartxt);//se agrega la letra al diccionario
                    Comparartxt = "";//comparador regresa a vacío


                    retrn = 0;//condición regresa a 0 para no entrar en el if de repetidos

                }

            }
            return compresion;
        }

        public static void commandReader(string action, Persona  person, AVL<Persona> arbol)
        {
            
            if (action == "INSERT")
            {
                try
                {
                    string nombre = person.name;
                    string dpi = Convert.ToString(person.DPI);
                    string descomprimidos = string.Join(" ", person.companies);
                    string comprimidos = compression(descomprimidos);
                    Lcomprimidos.Add($"DPI:{dpi} \n Nombre: {nombre} \n Compañias comprimidas:\n {comprimidos}\n");
                    Ldescomprimidos.Add($"DPI:{dpi} \n Nombre: {nombre} \n Compañias descomprimidas:\n {descomprimidos}\n");
                    arbol.Add(person);
                    //Console.WriteLine("Insert hecho");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("El error es " + ex);
                    throw;
                }


            }
            else if (action == "DELETE")
            {
                try
                {
                    arbol.Remove(person);
                    //Console.WriteLine("Delete hecho");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("El error es " + ex);
                    throw;
                }

            }
            else if (action == "PATCH")
            {
                try
                {
                    arbol.Update(person, person.DPI);
                    //Console.WriteLine("Patch hecho");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("El error es "+ ex);
                    throw;
                }
                
            }
        }

        public static void GuardarArbolEnJsonl(AVL<Persona> arbol, string filePath)
        {
            try
            {
                List<string> jsonLines = new List<string>();

                List<Persona> elementos = arbol.ObtenerLista();

                foreach (var persona in elementos)
                {
                    string jsonData = JsonConvert.SerializeObject(persona);
                    jsonLines.Add($"{jsonData}");
                }

                File.WriteAllLines(filePath, jsonLines);
                Console.WriteLine($"\nÁrbol guardado en '{filePath}'\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar el árbol en el archivo JSONL: " + ex.Message);
            }
        }


        public static string personaBuscada(long dpiABuscar)
        {
            Nodo<Persona> nodoEncontrado = arbol.GetByDPI(dpiABuscar);

            if (nodoEncontrado != null)
            {
                Persona personaEncontrada = nodoEncontrado.elvalor;
                string compañias = string.Join(" ", personaEncontrada.companies);
                return ($"DPI: {personaEncontrada.DPI} \nNombre: {personaEncontrada.name} \nNacimiento: {personaEncontrada.datebirth} \nDireccion: {personaEncontrada.address}\n ");
            }
            else
            {
                return ($"No se encontró un nodo con DPI: {dpiABuscar}");
            }
            
        }




    }
}
