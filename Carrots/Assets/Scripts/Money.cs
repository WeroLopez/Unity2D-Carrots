using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Money {
    /// <summary>
    /// Clase estatica que controla el dinero del juego y las actividades.
    /// </summary>
    public static class MoneyManager {
        /// <summary>
        /// dinero total del juego
        /// </summary>
        public static float TotalMoney = 0;
        /// <summary>
        /// dinero total de la actividad
        /// </summary>
        public static float ActivityMoney = 0;
        /// <summary>
        /// game object del texto de la ui en la que se muerstra el dinero de la actividad.
        /// </summary>
        public static GameObject go;
        public static int day = 0;
        public static float[] moneyByDay = new float[4];
        //public static float[] moneyByDay = { 350f,450f,656f,253f };
        public static float bonusMultiplier = 1.0f;
        /// <summary>
        /// Añade a la cantidad de dinero de la actividad actual, tambien refresca el contador de dinero de la pantalla.
        /// Para disminuir la cantidad de dinero usar un money negativo.
        /// </summary>
        /// <param name="money">Cantidad de dinero a sumar </param>
        public static void AddActivityMoney(float money) {
            ActivityMoney += money * bonusMultiplier;
            RefeshCounter();
        }
        /// <summary>
        /// Finaliza la actividad actual agregando el dinero de la actividad al dinero total.
        /// Regresa el dinero de la actividad a 0
        /// </summary>
        public static void EndActivity() {
            TotalMoney += ActivityMoney;
            moneyByDay[day] = ActivityMoney;
            ActivityMoney = 0;
            day = day == 3 ? 0 : day + 1;
        }
        /// <summary>
        /// Hace que el contador de la pantalla se actualize con el valor del dinero de la actividad actual.
        /// Usa un formato de $ 000.00
        /// </summary>
        private static void RefeshCounter() {
            //Debug.Log("hello");
            go.GetComponent<Text>().text = "$ " + ActivityMoney.ToString("000.00");
        }
        /// <summary>
        /// Cambia el go del contador de dinero al contador actual, se tiene que iniciar cada nueva escena,
        /// se recomienda ponder en el Start() o algo por el estilo de la clase que va a manejar el dinero de cada actividad.
        /// </summary>
        public static void IniciateMoney() {
            go = GameObject.Find("MoneyCount");
        }

    }
}
