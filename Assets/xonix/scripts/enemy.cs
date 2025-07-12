using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int x, y;
    public int dx = 1, dy = 0; // начальное направление
    public xonix gameManager;
    public float moveDelay = 0.2f;

    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        StartCoroutine(MoveEnemy());
    }

    IEnumerator MoveEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveDelay);
            Harakat();
            UpdatePosition();
        }
    }

    void Harakat()
    {
        int newX = x + dx;
        int newY = y + dy;

        if (newX < 0 || newX >= gameManager.satr || newY < 0 || newY >= gameManager.ustun ||
            gameManager.koord[newX, newY] == xonix.chegara)
        {
            // Если есть препятствие — выбрать случайное новое направление
            ChangeDirection();
            return;
        }

        // Если наступил на линию — игрок теряет жизнь
        if (gameManager.koord[newX, newY] == xonix.chiziq)
        {
            gameManager.minus1(); // уменьшить жизнь игрока
        }

        // Если пустое место — изменить направление
        if (gameManager.koord[newX, newY] == xonix.boshjoy)
        {
            ChangeDirection();
            return;
        }

        x = newX;
        y = newY;
    }

    void UpdatePosition()
    {
        // Обновить позицию в UI
        float katakx = gameManager.katakx;
        float kataky = gameManager.kataky;
        rt.localPosition = new Vector2((-gameManager.satr / 2 + x) * katakx, (-gameManager.ustun / 2 + y) * -kataky);
    }

    void ChangeDirection()
    {
        int[,] directions = new int[4, 2] { { 1, 1 }, { -1, 1 }, { -1, 1 }, { -1, -1 } };
        System.Random rand = new System.Random();
        int tries = 0;

        while (tries < 10)
        {
            int index = rand.Next(0, 4);
            int nx = x + directions[index, 0];
            int ny = y + directions[index, 1];

            if (nx >= 0 && nx < gameManager.satr && ny >= 0 && ny < gameManager.ustun &&
                gameManager.koord[nx, ny] != xonix.chegara)
            {
                dx = directions[index, 0];
                dy = directions[index, 1];
                return;
            }
            tries++;
        }

        dx = 0;
        dy = 0;
    }
}