using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameController : MonoBehaviour {
    public Camera cam;
    public UnityEngine.GameObject[] balls;
    // en este juego se trata de ver cuantas balls se cachan en un tiempo determinado
    public float timeLeft;
    public Text timerText;
    public UnityEngine.GameObject gameOverText;
   // public GameObject restartButton;
   // public GameObject splashScreen;
   // public GameObject startButton;
    public HatController hatController;
    public Text scoreText;

    public static GameController instance;

    // esto solo si para probar los eventos
    public UnityEvent DamageEvent;
    public UnityEvent RepairEvent;
    //public UnityEvent EspadaEvent;

 //   public Transform[] spawnPoints;

    private bool playing;
    private float timeBefore;

    // ancho max del área de juego
    private float maxWidth;
    // se necesita conocer las fronteras del juego en el world game
    // la camara es la nos ayudará a determinar el área de trabajo. Cuanto es lo que ve la camara.
    // para hacerlo se necesita encontrar un punto en el borde de la pantalla en el área de la pantalla 
    // para pasarlo al world space. Ese punto indica el borde o frontera del área de juego.
    // se puede encontrar ese tamaño usando el screen class
    // Screen tiene un high y un width
    // este tamaño se se necesita calcular una vez, así que se hace en start()
    // OJO esto se puede hacer porque se considera una cámara ORTOGONAL
    // es punto buscado es upperCorner

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    // Use this for initialization
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        playing = false;
        timeBefore = timeLeft;
        Vector3 upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
        Vector3 targetWidth = cam.ScreenToWorldPoint(upperCorner);
        // se necesita considerar el ancho del sombrero para que no salga de la escena.
        // para eso se necesita considerar el renderizador, ya que es el único que conoce el tamaño 
        // del objeto/imagen que está renderizando. Así se hace de forma dinámica.
        // el renderer tiene bounds y este tiene extents, y buscamos el extent x

        float ballWidth = balls[0].GetComponent<Renderer>().bounds.extents.x;
        maxWidth = targetWidth.x - ballWidth;
        timerText.text = "Tiempo restante:\n" + Mathf.RoundToInt(timeLeft);

        // aqui viene una modificación. a ver si funciona que inicia el juego luego, luego

       // splashScreen.SetActive(false);
       // startButton.SetActive(false);
        // aquí se indica a HatController que ya podemos controlar el Hat, porque ya va a iniciar el juego
        // en HatController la función ToggleControl pone la variable canControl en true y ya
       /* hatController.ToggleControl(true);
        //Random.seed = System.DateTime.Now.Second;
        StartCoroutine(Spawn());*/
    }

    // FixedUpdate for Physics pero tambien en un tiempo determina fijo. Aquí se decrementa la variable timeLeft
    // Esto está bien considerarlo cuando se quiera hacer un decremento de tiempo, como para un reloj
    // que no necesita actualizarse con cada frame, sino de forma regular en el tiempo
    void FixedUpdate()
    {
        if (playing)
        {
            timeBefore = timeLeft;
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                timeLeft = 0;
                timeBefore = timeLeft;
            }
            timerText.text = "Tiempo restante:\n" + Mathf.RoundToInt(timeLeft);
        }
    }

    // se crea la función para iniciar el juego desde la splash scene
    // se quiere desactivar la SplashScreen y el startButton
    // asi como iniciar la corrutina Spawn
    // se necesita tener referencias a esas variables (que son GameObjects) y que sean públicas -se hace arriba
    // ESTA RUTINA YA NO LA ESTOY USANDO. No, si la estoy usando
    public void StartGame()
    {
       // splashScreen.SetActive(false);
       // startButton.SetActive(false);
        // aquí se indica a HatController que ya podemos controlar el Hat, porque ya va a iniciar el juego
        // en HatController la función ToggleControl pone la variable canControl en true y ya
        // lo voy a poner en update()
        
        hatController.ToggleControl(true);
        playing = true;
        //Random.seed = System.DateTime.Now.Second;
        //StartCoroutine(Spawn());
        

    }
    // la rutina de Update no estaba. Estoy modificando esto.
    public void Update()
    {
        if (playing == false) return;
        if(Mathf.RoundToInt(timeLeft) > 0)
        {
            /*
            hatController.ToggleControl(true);
            playing = true;
            */
            if (Mathf.RoundToInt(timeLeft) != Mathf.RoundToInt(timeBefore + 0.03f))
            {
                UnityEngine.GameObject ball = balls[Random.Range(0, balls.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-(maxWidth), maxWidth), transform.position.y, 0.0f);   // ojo aqui cambié y puse -2
                                                                                                                        // para Rotation, recordar que eso es un quartinion y no hay rotation. 
                                                                                                                        // eso se representa con Quaternion.identity
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(ball, spawnPosition, spawnRotation);
            }
            
        }
        else
        {
            //gameOverText.SetActive(true);
            playing = false;

            GameOverHat();
            //UIEvents.instance.LoadCroquisScene();
        }

    }

    // necesitamos engendrar balls dentro de la escena. Spawn. Para ello se usarán corutinas con Ienumerator.
    // y esa corutina va a instanciar balls.
    IEnumerator Spawn()
    {
        // Al inicio se hace una espera para que el juego no emmpiece inmediatamente.
        // así que se hace un yield de espera
        yield return new WaitForSeconds( 2.0f);
        playing = true;
       while (timeLeft > 0)
        {
            // en un loop se deben generar tantas balls como se puedan y no solo una vez

            // se obtiene el gameobject
            // GameObject ball = balls[Random.Range(0, balls.Length)];

            // la position inicial de la pelota estará justo arriba del escenario
            // donde x está entre (-maxWidth, maxWidth)
            // y es la posicion del gameobject en y, dado por su transform.position
            // y z es 0
            UnityEngine.GameObject ball = balls[Random.Range(0, balls.Length)];
//           Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            Vector3 spawnPosition = new Vector3(
                Random.Range(-(maxWidth), maxWidth),
                transform.position.y,
                0.0f);   // ojo aqui cambié y puse -2
            // para Rotation, recordar que eso es un quartinion y no hay rotation. 
            // eso se representa con Quaternion.identity
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(ball, spawnPosition, spawnRotation);
            // se para esta corrutina con un yield
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        } 
        yield return new WaitForSeconds(2.0f);
        gameOverText.SetActive(true);
        yield return new WaitForSeconds(2.0f);

        //restartButton.SetActive(true);
        // cuando termina el tiempo se manda llamar a la escena del croquis
        UIEvents.instance.LoadCroquisScene();
    }

    private void GameOverHat()
    {
        //int score;
        if (!playing)
        {
            BlackArea.Show();
            //scoreText = GameObject.Find("TerminasteDialogHat").transform.Find("Message").GetComponent<Text>();
           // GameObject scoreGO = GameObject.Find("TerminasteDialogHat");
           // if (scoreGO == null) return;
           // scoreText = scoreGO.transform.Find("Message").GetComponent<Text>();
            //score = Score.instance.GetScoreHat();
            /*
            if (scoreText == null)
            {
                return;
            }
            */
           // scoreText.text = "Total de alimentos permitidos atrapados: " + score;
            hatController.ToggleControl(false);
            hatController.DestroyHat();
            // GameObject.Find("TerminasteDialogHat").GetComponent<Dialog>().Show(false);
            GanoOPerdio();
        }
    }

    private void Perdiste(int score)
    {
        //Play level completed sound effect
        // AudioClips.instance.PlayCompletedSFX();

        //Show black area
        //BlackArea.Show();
        TerminasteDialog.instance.SetDialog("PerdisteDialog");
        TerminasteDialog.instance.SetScore(score);
        //GameObject.Find("PerdisteDialog").GetComponent<Dialog>().Show(false);
        Debug.Log("Perdiste");
    }

    private void Ganaste(int score)
    {
        //Play level completed sound effect
        //AudioClips.instance.PlayCompletedSFX();

        //Show black area
        // BlackArea.Show();
        TerminasteDialog.instance.SetDialog("GanasteDialog");
        TerminasteDialog.instance.SetScore(score);
        //GameObject.Find("GanasteDialog").GetComponent<Dialog>().Show(false);
        Debug.Log("Ganaste");
    }

    public void GanoOPerdio()
    {
        int score;

        score = Score.instance.GetScoreHat(); ;
        if (score >= 25)
        {
            // codigo de cuando gana
            Ganaste(score);
            // a ver si funcionan los eventos
            RepairEvent.Invoke();
            //EspadaEvent.Invoke();
        }
        else
        {
            // codigo de cuando pierde
            Perdiste(score);
            // a ver si funcionan los eventos
            DamageEvent.Invoke();
        }
        //Show black area
        //BlackArea.Show();

        Debug.Log("Gano o perdio");
    }

}
