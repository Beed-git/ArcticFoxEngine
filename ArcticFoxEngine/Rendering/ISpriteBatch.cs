﻿using ArcticFoxEngine.Math;

namespace ArcticFoxEngine.Rendering;

public interface ISpriteBatch
{
    public void Start();
    public void DrawRectangle(Rectangle destination, Color color);
    public void End();
}