using UnityEngine;
using System.Collections;

public class Button {

    public Rect rect;
    public Vector2 size, position, direction;//direction will move 1 pix per unit, (ie. Vec2(1, 0), will move it 1 pixel to the right when opened)
    public bool open = false;

    //Create Menu with custom size
    public Button(Vector2 position, Vector2 size, Vector2 direction) {
        this.size = size;
        this.position = position;
        this.direction = direction;

        rect = new Rect(position, size);
    }

    //Create Menu with default size of 25
    public Button(Vector2 position, Vector2 direction) {
        size = new Vector2(25, 25);
        this.position = position;
        this.direction = direction;

        rect = new Rect(position, size);
    }

    public void Toggle() {
        if (open) {
            Close();
        } else if (!open) {
            Open();
        }
    }

    public void Open() {
        DirectionCheck();

        if (rect.size == size) {
            rect.size += direction;
        }

        open = true;
    }

    public void Close() {
        if (rect.size != size) {
            rect.size = size;
            rect.position = position;
        }
        //Make sure the direction is still pointing in the right direction after moving the position
        direction.x *= flipped.x;
        direction.y *= flipped.y;

        open = false;
    }

    Vector2 flipped = Vector2.one;
    void DirectionCheck() {
        if (direction.x < 0) {
            rect.position += new Vector2(direction.x, 0);
            direction.x *= -1;//flip to positive
            flipped.x = -1;
        }
        if (direction.y < 0) {
            rect.position += new Vector2(0, direction.y);
            direction.y *= -1;//flip to positive
            flipped.y = -1;
        }
    }
}
