using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
   public int x;
   public int y;

   //Konstruktor
   public Point(int inX, int inY)
   {
   		x = inX;
   		y = inY;
   }

   //Kozvetlen ertekvaltoztato metodusok
   public void Mult( int value)
   {
      this.x *= value;
      this.y *= value;
   }

   public void Add(Point value)
   {
      this.x += value.x;
      this.y += value.y;
   }

   //Vector es Pont konverzio easy kezelese
   public static Point FromVector(Vector2 v)
   {
      return new Point((int)v.x, (int)v.y);
   }

   public static Point FromVector(Vector3 v)
   {
      return new Point((int)v.x, (int)v.y);
   }

   public Vector2 ToVector()
   {
      return new Vector2(this.x,this.y);
   }

   //Pontokkal kapcsolatos muveleteket
   public static Point MultiplyPoint(Point point, int value)
   {
      return new Point(point.x * value, point.y * value);
   }

   public static Point AddPoints(Point firstpoint, Point secondpoint)
   {
      return new Point(firstpoint.x + secondpoint.x, firstpoint.y + secondpoint.y);
   }

   public static Point ClonePoints(Point p)
   {
      return new Point(p.x,p.y);
   }

   public bool EqualPoints(Point p)
   {
      return (this.x == p.x && this.y == p.y);
   }

   //Szomszedos pontok lekeresehez
   public static Point Zero
   {
      get {return new Point(0,0);}
   }

   public static Point One
   {
      get {return new Point(1,1);}
   }

   public static Point Up
   {
      get {return new Point(0,1);}
   }

      public static Point Down
   {
      get {return new Point(0,-1);}
   }

   public static Point Left
   {
      get {return new Point(-1,0);}
   }

   public static Point Right
   {
      get {return new Point(1,0);}
   }
}
