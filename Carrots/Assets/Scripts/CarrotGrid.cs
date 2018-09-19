using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Money;

#pragma warning disable 0649

/// <summary>
/// Clase que controla el grid de las zanahorias, zanahorias doradas y ardillas
/// </summary>
public class CarrotGrid : MonoBehaviour {

    //game objects
    [SerializeField]
    GameObject carrot, goldenCarrot, hole, squirrel;
    
    //grid
    [SerializeField]
    int gridSizeX, gridSizeY;
    public GameObject[,] grid;
    int playerPosX, playerPosY;
    ArrayList carrotsInArea;
    int[] carrotsForRandom;

    //Sprites
    [SerializeField]
    Sprite carrot_0, carrot_1, carrot_2, carrot_3, carrot_4;
    [SerializeField]
    Sprite GoldenCarrot_0, GoldenCarrot_1, GoldenCarrot_2, GoldenCarrot_3, GoldenCarrot_4;
    Sprite[] carrotSprites, goldenCarrotSprites;

    //Tiume 
    [SerializeField]
    Text timeText, endText;
    float totalTime;

    //GoldenCarrot
    float goldenCarrotTime, returnGoldenCarrotTime;
    bool goldenCarrotActive;
    int goldenCarrotPosX, goldenCarrotPosY;

    //Squirrel
    int squirrelPosX, squirrelPosY;
    float squirrelAppearTime, squirrelDisappearTime, squirrelCarrotPullTime, squirrelStartPullTime;
    bool squirrelActive;
    int squirrelCarrotSprite;

    //UI
    [SerializeField]
    Text moneyText;

    //Fences
    [SerializeField]
    int difficulty;
    [SerializeField]
    GameObject horizontalObstacle, verticalObstacle;
    GameObject obstacle;

    // Use this for initialization
    void Start () {
        //Iniciar el grid de zanahorias
        grid = new GameObject[gridSizeX, gridSizeY];
        for(int i = 0; i<gridSizeX; i++) {
            for (int j = 0; j < gridSizeY; j++) {
                grid[i, j] = Instantiate(carrot);
                grid[i, j].name = "Carrot:" + i + "," + j;
                grid[i, j].transform.position = transform.position + new Vector3(i*2.2f,-j*1.4f -.1f,0);
                grid[i, j].GetComponent<SpriteRenderer>().sortingLayerName = (j + 1) + "_Carrot";
            }
        }
        //Iniciar la posición del jugador respecto al grid
        playerPosX = 0;
        playerPosY = 0;
        carrotsInArea = new ArrayList();
        //Tiempo para la primera zanahoria de oro
        goldenCarrotTime = Mathf.Round(Random.Range(5, 8));
        goldenCarrotActive = false;
        //Tiempo para la primera ardilla
        squirrelAppearTime = Mathf.Round(Random.Range(5, 8));
        squirrelStartPullTime = 1;
        squirrelActive = false;
        squirrelCarrotSprite = 0;
        //Sprites de las zanahorias
        carrotSprites = new Sprite[] { carrot_0, carrot_1, carrot_2, carrot_3, carrot_4 };
        goldenCarrotSprites = new Sprite[] { GoldenCarrot_0, GoldenCarrot_1, GoldenCarrot_2, GoldenCarrot_3, GoldenCarrot_4 };

        CreateObstacles();

        totalTime = 60f;
        MoneyManager.IniciateMoney();
    }

    void Update() {
        //Contar el tiempo total
        totalTime -= Time.deltaTime;
        timeText.text = totalTime.ToString("00");
        if(totalTime <= 0) {
            endText.gameObject.SetActive(true);
            MoneyManager.EndActivity();
        }
        //Contar el tiempo para las zanahorias de oro
        goldenCarrotTime -= Time.deltaTime;
        if(goldenCarrotTime <= 0f) {
            GoldenCarrot();
            goldenCarrotTime = Mathf.Round(Random.Range(5, 8));
        }
        if(goldenCarrotActive) {
            //Contar el tiempo para que desaparezca la zanahoria de oro
            returnGoldenCarrotTime -= Time.deltaTime;
            if (returnGoldenCarrotTime < 0f) {
                goldenCarrotTime = Mathf.Round(Random.Range(5, 8));
                ReturnGoldenCarrot();
            }
        }
        //Contar el tiempo para las ardillas
        squirrelAppearTime -= Time.deltaTime;
        if (squirrelAppearTime <= 0f) {
            SquirrelAppear();
            squirrelAppearTime = 100;
        }
        if (squirrelActive) {
            squirrelStartPullTime -= Time.deltaTime;
            if (squirrelStartPullTime <= 0f) {
                SquirrelStartPull();
                //Contar el tiempo para que desaparezca la ardilla
                squirrelDisappearTime -= Time.deltaTime;
                squirrelCarrotPullTime -= Time.deltaTime;
                //Cambiar el sprite de la zanahoria que se esta llevando la ardilla
                if (squirrelCarrotPullTime < 0f) {
                    squirrelCarrotPullTime = .2f;
                    squirrelCarrotSprite = squirrelCarrotSprite == 0 ? 1 : 0;
                    grid[squirrelPosX, squirrelPosY].GetComponent<SpriteRenderer>().sprite = carrotSprites[squirrelCarrotSprite];
                }
                if (squirrelDisappearTime < 0f) {
                    SquirrelDisappear();
                }
            }
        }
    }
    /// <summary>
    /// Aparece una ardilla
    /// </summary>
    void SquirrelAppear() {
        if (SearchCarrotsInArea() > 0) {
            squirrelActive = true;
            int carrotToTake;
            string name;
            do {
                carrotToTake = (int)Mathf.Round(Random.Range(0, carrotsInArea.Count));
                name = (string)carrotsInArea[carrotToTake];
                squirrelPosX = int.Parse(name.Substring(7, 1));
                squirrelPosY = int.Parse(name.Substring(9));
            } while ((squirrelPosX == playerPosX && squirrelPosY == playerPosY) || (squirrelPosX == goldenCarrotPosX && squirrelPosY == goldenCarrotPosY));
            squirrel.transform.position = grid[squirrelPosX, squirrelPosY].transform.position + new Vector3(-.5f, 0f, 0f);
            squirrel.GetComponent<BoxCollider2D>().enabled = false;
            grid[squirrelPosX, squirrelPosY].GetComponent<CircleCollider2D>().enabled = false;
            squirrel.GetComponent<Animator>().SetTrigger("In");
            squirrel.GetComponent<SpriteRenderer>().sortingLayerName = (squirrelPosY + 1) + "_Squirrel";

            squirrelDisappearTime = 3f;
            squirrelCarrotPullTime = .2f;
        }
    }
    void SquirrelStartPull() {
        squirrel.GetComponent<BoxCollider2D>().enabled = true;
    }
    /// <summary>
    /// Desaparece una ardilla, llevandose la zanahoria, quitandole dinero al jugador
    /// </summary>
    public void SquirrelDisappear() {
        squirrelActive = false;
        squirrelStartPullTime = 1;
        squirrelAppearTime = Mathf.Round(Random.Range(6, 9));
        squirrel.GetComponent<BoxCollider2D>().enabled = false;
        squirrel.GetComponent<Animator>().SetTrigger("Out");
        DestroyCarrot(squirrelPosX, squirrelPosY, false);
        MoneyManager.AddActivityMoney(-30f);
    }
    /// <summary>
    /// El jugador ahuyento a la ardilla
    /// </summary>
    public void SquirrelScared() {
        squirrelActive = false;
        squirrelStartPullTime = 1;
        squirrelAppearTime = Mathf.Round(Random.Range(6, 9));
        squirrel.GetComponent<BoxCollider2D>().enabled = false;
        squirrel.GetComponent<Animator>().SetTrigger("Scared");
        grid[squirrelPosX, squirrelPosY].GetComponent<SpriteRenderer>().sprite = carrotSprites[0];
        grid[squirrelPosX, squirrelPosY].GetComponent<CircleCollider2D>().enabled = true;
    }
    /// <summary>
    /// Aparece una zanahoria de oro
    /// </summary>
    void GoldenCarrot() {
        if (SearchCarrotsInArea() > 0) {
            goldenCarrotActive = true;
            int carrotToDestroy;
            string name;
            do {
                carrotToDestroy = (int)Mathf.Round(Random.Range(0, carrotsInArea.Count));
                name = (string)carrotsInArea[carrotToDestroy];
                goldenCarrotPosX = int.Parse(name.Substring(7, 1));
                goldenCarrotPosY = int.Parse(name.Substring(9));
            } while (goldenCarrotPosX == playerPosX && goldenCarrotPosY == playerPosY);
            Destroy(grid[goldenCarrotPosX, goldenCarrotPosY]);
            grid[goldenCarrotPosX, goldenCarrotPosY] = Instantiate(goldenCarrot);
            grid[goldenCarrotPosX, goldenCarrotPosY].name = "Golden:" + goldenCarrotPosX + "," + goldenCarrotPosY;
            grid[goldenCarrotPosX, goldenCarrotPosY].transform.position = transform.position + new Vector3(goldenCarrotPosX * 2.2f, -goldenCarrotPosY * 1.4f -.05f, 0);
            grid[goldenCarrotPosX, goldenCarrotPosY].GetComponent<SpriteRenderer>().sortingLayerName = (goldenCarrotPosY + 1) + "_Carrot";

            returnGoldenCarrotTime = 4f;
        }
    }
    /// <summary>
    /// La zanahoria de oro regresa a su estado normal ya que el jugador no fue rapido
    /// </summary>
    private void ReturnGoldenCarrot() {
        goldenCarrotActive = false;
        Destroy(grid[goldenCarrotPosX, goldenCarrotPosY]);
        grid[goldenCarrotPosX, goldenCarrotPosY] = Instantiate(carrot);
        grid[goldenCarrotPosX, goldenCarrotPosY].name = "Carrot:" + goldenCarrotPosX + "," + goldenCarrotPosY;
        grid[goldenCarrotPosX, goldenCarrotPosY].transform.position = transform.position + new Vector3(goldenCarrotPosX * 2.2f, -goldenCarrotPosY * 1.4f -.1f, 0);
        goldenCarrotPosX = -1; goldenCarrotPosY = -1;
        return;
    }
    /// <summary>
    /// Quitar una zanahoria del grid, dejando un hoyo
    /// </summary>
    /// <param name="x">Posición en X de la zanahoria</param>
    /// <param name="y">Posición en Y de la zanahoria</param>
    /// <param name="golden">Si la zanahoria es normal: false, si es de oro: true</param>
    public void DestroyCarrot(int x, int y, bool golden) {
        if (golden) {
            goldenCarrotActive = false;
            goldenCarrotPosX = -1; goldenCarrotPosY = -1;
        }
        Destroy(grid[x, y]);
        grid[x, y] = Instantiate(hole);
        grid[x, y].name = "Holeee:" + x + "," + y;
        grid[x, y].transform.position = transform.position + new Vector3(x * 2.2f, -y * 1.4f -.1f, 0);
        grid[x, y].GetComponent<SpriteRenderer>().sortingLayerName = (y + 1) + "_Carrot";
    }
    /// <summary>
    /// Cambia el sprite de la zanahoria que esta siendo sacada
    /// </summary>
    /// <param name="x">Posición en X de la zanahoria</param>
    /// <param name="y">Posición en Y de la zanahoria</param>
    /// <param name="sprite">Sprite actual de la zanahoria (del 0 al 4)</param>
    public void PullCarrot(int x, int y, int sprite) {
        if (grid[x, y].name.StartsWith("C")) {
            grid[x, y].GetComponent<SpriteRenderer>().sprite = carrotSprites[sprite + 1 > 4 ? sprite : sprite + 1];
        }
        else {
            grid[x, y].GetComponent<SpriteRenderer>().sprite = goldenCarrotSprites[sprite + 1 > 4 ? sprite : sprite + 1];
        }
    }
    /// <summary>
    /// Regresar una zanahoria a su posición inicial, sea normal o de oro
    /// </summary>
    /// <param name="x">Posición en X de la zanahoria</param>
    /// <param name="y">Posición en Y de la zanahoria</param>
    public void ReturnCarrotToOriginalPosition (int x, int y) {
        if (grid[x, y].name.StartsWith("C")) {
            grid[x, y].GetComponent<SpriteRenderer>().sprite = carrotSprites[0];
        }
        else {
            grid[x, y].GetComponent<SpriteRenderer>().sprite = goldenCarrotSprites[0];
        }
    }
    /// <summary>
    /// Saber la posición del jugador
    /// </summary>
    /// <param name="x">Posición en X</param>
    /// <param name="y">Posición en Y</param>
    public void PlayerPosition(int x, int y) {
        playerPosX = x;
        playerPosY = y;
    }
    /// <summary>
    /// Buscar la cantidad de zanahorias que tiene el jugador en un radio de 2 zanahorias
    /// </summary>
    /// <returns>Cantidad de zanahorias encontradas</returns>
    private int SearchCarrotsInArea() {
        carrotsInArea.Clear();
        int xi         = playerPosX - 2 < 0 ?         0 : playerPosX - 2;
        int xf         = playerPosX + 3 > gridSizeX ? gridSizeX : playerPosX + 3;
        int yiOriginal = playerPosY - 2 < 0 ?         0 : playerPosY - 2;
        int yf         = playerPosY + 3 > gridSizeY ? gridSizeY : playerPosY + 3;
        for (; xi < xf; xi++) {
            for (int yi = yiOriginal; yi < yf; yi++) {
                if(grid[xi, yi].name.StartsWith("C")) {
                    carrotsInArea.Add(grid[xi, yi].name);
                }
            }
        }
        return carrotsInArea.Count;
    }
    /// <summary>
    /// Cambiar la sort layer de la zanahoria que esta pisando el jugador
    /// </summary>
    /// <param name="layer">Nombre de la sort layer</param>
    public void SortLayer(string layer) {
        grid[playerPosX, playerPosY].GetComponent<SpriteRenderer>().sortingLayerName = (playerPosY + 1) + layer;
    }
    /// <summary>
    /// Crea obstáculos en el campo dependiendo la dificultad
    /// </summary>
    void CreateObstacles() {
        for (int i=0; i < gridSizeX; i++) {
            for (int j = 0; j < gridSizeY; j++) {
                if (j != gridSizeY - 1) {
                    int r = (int)Mathf.Round(Random.Range(0, 10));
                    if(r < difficulty) {
                        obstacle = Instantiate(horizontalObstacle);
                        obstacle.transform.position = transform.position + new Vector3(i * 2.2f, -j * 1.4f - .9f, 0);
                        obstacle.GetComponent<SpriteRenderer>().sortingLayerName = (j + 1) + "_Fence";
                    }
                }
                if (i != gridSizeX - 1) {
                    int r = (int)Mathf.Round(Random.Range(0, 10));
                    if (r < difficulty) {
                        obstacle = Instantiate(verticalObstacle);
                        obstacle.transform.position = transform.position + new Vector3(i * 2.2f + 1.1f, -j * 1.4f -.2f, 0);
                        obstacle.GetComponent<SpriteRenderer>().sortingLayerName = (j + 1) + "_Fence";
                    }
                }
            }
        }
    }

}
