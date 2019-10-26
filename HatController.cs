using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatController : MonoBehaviour {
    public Camera cam;
    // la velocidad
    public float speed;

    // x es el valor del eje horizontal
    float x;

    // Vector2 move para poner la velocidad y la dirección en el Rigidbody2D
    Vector2 move;

    // como se cambiará la velocidad del Rigidbody2D necesitamos una referencia a este.
   // Rigidbody2D rb;

    // ancho max del área de juego
    private float maxWidth;
    // podemos controlar el hat?
    private bool canControl;

    // se necesita conocer las fronteras del juego en el world game
    // la camara es la nos ayudará a determinar el área de trabajo. Cuanto es lo que ve la camara.
    // para hacerlo se necesita encontrar un punto en el borde de la pantalla en el área de la pantalla 
    // para pasarlo al world space. Ese punto indica el borde o frontera del área de juego.
    // se puede encontrar ese tamaño usando el screen class
    // Screen tiene un high y un width
    // este tamaño se se necesita calcular una vez, así que se hace en start()
    // OJO esto se puede hacer porque se considera una cámara ORTOGONAL
    // es punto buscado es upperCorner

	// Use this for initialization
	void Start () {
		if (cam == null)
        {
            cam = Camera.main;
        }

        // lo volví a cambiar para que empiece cuando empieza el juego, con el toggle en true
        canControl = false;

        Vector3 upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
        Vector3 targetWidth = cam.ScreenToWorldPoint(upperCorner);


        // se necesita considerar el ancho del sombrero para que no salga de la escena.
        // para eso se necesita considerar el renderizador, ya que es el único que conoce el tamaño 
        // del objeto/imagen que está renderizando. Así se hace de forma dinámica.
        // el renderer tiene bounds y este tiene extents, y buscamos el extent x

        float hatWidth = GetComponent<Renderer>().bounds.extents.x;
        maxWidth = targetWidth.x - hatWidth;
 

	}
	
	// Update is called once per frame. FixedUpdate se llama más veces dentro de un mismo frame
	void FixedUpdate () {
        if (canControl)
        {
            Vector3 rawPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPosition = new Vector3(rawPosition.x * speed, -2.5f, 0.0f);
            // se necesita clamp amarrar la posición del punto del borde y el ancho del área de juego
            // la posición target estará entre -maxWidth y maxWidth
            // esto funciona por la forma en que se construyó la escena:
            // todo está centrado en el eje xy = 0 y Z = 
            float targetWidth = Mathf.Clamp(targetPosition.x, -maxWidth, maxWidth);
            // x se moverá entonces entre los bordes de la escena y se mantiene y, z
            targetPosition = new Vector3(targetWidth, targetPosition.y, targetPosition.z);
            GetComponent<Rigidbody2D>().MovePosition(targetPosition);
        }

	}

    // se agrega una función pública que hará que conControl tome el valor que se le asigna por
    // medio de la variable de entrada

    public void ToggleControl(bool toggle)
    {
        canControl = toggle;
    }

   public void ApagaHat()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        GetComponent<SpriteRenderer>().color = tmp;
    }

    public void DestroyHat()
    {
        Destroy(gameObject);
    }
}
