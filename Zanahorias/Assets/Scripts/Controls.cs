using UnityEngine;

namespace CoreGame {
    public static class Controls {
        static bool LeftJoystickMoved;
        static bool RightJoystickMoved;
        static bool PadMoved;
        static bool LeftTriggerMoved;
        static bool RightTriggerMoved;

        /// <summary>
        /// Regresa true si el input A_Button es presionado.
        /// </summary>
        public static bool button_A {
            get { return Input.GetButton("A_Button"); }
        }
        /// <summary>
        /// Regresa true si el input B_Button es presionado.
        /// </summary>
        public static bool button_B {
            get { return Input.GetButton("B_Button"); }
        }
        /// <summary>
        /// Regresa true si el input X_Button es presionado.
        /// </summary>
        public static bool button_X {
            get { return Input.GetButton("X_Button"); }
        }
        /// <summary>
        /// Regresa true si el input Y_Button es presionado.
        /// </summary>
        public static bool button_Y {
            get { return Input.GetButton("Y_Button"); }
        }
        /// <summary>
        /// Regresa true si el input Left_Button es presionado.
        /// </summary>
        public static bool button_Left {
            get { return Input.GetButton("Left_Button"); }
        }
        /// <summary>
        /// Regresa true si el input Right_Button es presionado.
        /// </summary>
        public static bool button_Right {
            get { return Input.GetButton("Right_Button"); }
        }
        /// <summary>
        /// Regresa true si el input Back_Button es presionado.
        /// </summary>
        public static bool button_Back {
            get { return Input.GetButton("Back_Button"); }
        }
        /// <summary>
        /// Regresa true si el input Start_Button es presionado.
        /// </summary>
        public static bool button_Start {
            get { return Input.GetButton("Start_Button"); }
        }

        /// <summary>
        /// Regresa un float restringido entre -1 y 1 del valor horizontal izquierdo.
        /// (Preferible usar la función LeftJoystick).
        /// </summary>
        /// <returns>Float</returns>
        public static float LeftHorizontal() {
            float r = 0.0f;
            r += Input.GetAxis("J_Left_Horizontal");
            r += Input.GetAxis("K_Left_Horizontal");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }
        /// <summary>
        /// Regresa un float restringido entre -1 y 1 del valor vertical izquierdo.
        /// (Preferible usar la función LeftJoystick).
        /// </summary>
        /// <returns>Float</returns>
        public static float LeftVertical() {
            float r = 0.0f;
            r += Input.GetAxis("J_Left_Vertical");
            r += Input.GetAxis("K_Left_Vertical");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }
        /// <summary>
        /// Regresa un Vector2 del valor horizonal y vertical izquierdo, verificando si el joystick fue movido.
        /// </summary>
        /// <returns></returns>
        public static Vector2 LeftJoystick() {
            if (LeftHorizontal() != 0 || LeftVertical() != 0) {
                LeftJoystickMoved = true;
            }
            return new Vector2(LeftHorizontal(), LeftVertical());
        }
        /// <summary>
        /// Verifica si el joystick izquierdo regresó al centro después de ser movido.
        /// </summary>
        /// <returns>Bool</returns>
        public static bool LeftJoystickReturnedCenter() {
            if (LeftJoystickMoved) {
                if (LeftHorizontal() == 0 && LeftVertical() == 0) {
                    LeftJoystickMoved = false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Regresa un float restringido entre -1 y 1 del valor horizontal derecho.
        /// (Preferible usar la función RightJoystick).
        /// </summary>
        /// <returns>Float</returns>
        public static float RightHorizontal() {
            float r = 0.0f;
            r += Input.GetAxis("J_Right_Horizontal");
            //r += Input.GetAxis("K_Right_Horizontal");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }
        /// <summary>
        /// Regresa un float restringido entre -1 y 1 del valor vertical derecho.
        /// (Preferible usar la función RightJoystick).
        /// </summary>
        /// <returns>Float</returns>
        public static float RightVertical() {
            float r = 0.0f;
            r += Input.GetAxis("J_Right_Vertical");
            //r += Input.GetAxis("K_Right_Vertical");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }
        /// <summary>
        /// Regresa un Vector2 del valor horizonal y vertical derecho, verificando si el joystick fue movido.
        /// </summary>
        /// <returns></returns>
        public static Vector2 RightJoystick() {
            if (RightHorizontal() != 0 || RightVertical() != 0) {
                RightJoystickMoved = true;
            }
            return new Vector2(RightHorizontal(), RightVertical());
        }
        /// <summary>
        /// Verifica si el joystick derecho regresó al centro después de ser movido.
        /// </summary>
        /// <returns>Bool</returns>
        public static bool RightJoystickReturnedCenter() {
            if (RightJoystickMoved) {
                if (RightHorizontal() == 0 && RightVertical() == 0) {
                    RightJoystickMoved = false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Regresa un float restringido entre -1 y 1 del valor horizonmtal del pad.
        /// </summary>
        /// <returns>Float</returns>
        public static float PadHorizontal() {
            float r = 0.0f;
            r += Input.GetAxis("Pad_Horizontal");
            //r += Input.GetAxis("K_Pad_Horizontal");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }
        /// <summary>
        /// Regresa un float restringido entre -1 y 1 del valor vertical del pad.
        /// </summary>
        /// <returns>Float</returns>
        public static float PadVertical() {
            float r = 0.0f;
            r += Input.GetAxis("Pad_Vertical");
            //r += Input.GetAxis("K_Pad_Vertical");
            return Mathf.Clamp(r, -1.0f, 1.0f);
        }
        /// <summary>
        /// Regresa un Vector2 del valor horizonal y vertical del pad, verificando si el pad fue movido.
        /// </summary>
        /// <returns></returns>
        public static Vector2 Pad() {
            if (PadHorizontal() != 0 || PadVertical() != 0) {
                PadMoved = true;
            }
            return new Vector2(PadHorizontal(), PadVertical());
        }
        /// <summary>
        /// Verifica si el pad regresó al centro después de ser movido.
        /// </summary>
        /// <returns>Bool</returns>
        public static bool PadReturnedCenter() {
            if (PadMoved) {
                if (PadHorizontal() == 0 && PadVertical() == 0) {
                    PadMoved = false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Regresa un float restringido entre 0 y 1 del valor del trigger izquierdo.
        /// </summary>
        /// <returns>Float</returns>
        public static float LeftTrigger() {
            float r = 0.0f;
            r += Input.GetAxis("Left_Trigger");
            //r += Input.GetAxis("K_Left_Trigger");
            r = Mathf.Clamp(r, 0, 1.0f);
            if (r != 0) {
                LeftTriggerMoved = true;
            }
            return r;
        }
        /// <summary>
        /// Verifica si el trigger izquierdo fue levantado.
        /// </summary>
        /// <returns>Bool</returns>
        public static bool LeftTriggerUp() {
            if (LeftTriggerMoved) {
                if (LeftTrigger() == 0) {
                    LeftTriggerMoved = false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Regresa un float restringido entre 0 y 1 del valor del trigger derecho.
        /// </summary>
        /// <returns>Float</returns>
        public static float RightTrigger() {
            float r = 0.0f;
            r += Input.GetAxis("Right_Trigger");
            //r += Input.GetAxis("K_Right_Trigger");
            r = Mathf.Clamp(r, 0, 1.0f);
            if (r != 0) {
                RightTriggerMoved = true;
            }
            return r;
        }
        /// <summary>
        /// Verifica si el trigger derecho fue levantado.
        /// </summary>
        /// <returns>Bool</returns>
        public static bool RightTriggerUp() {
            if (RightTriggerMoved) {
                if (RightTrigger() == 0) {
                    RightTriggerMoved = false;
                    return true;
                }
            }
            return false;
        }
    }
}