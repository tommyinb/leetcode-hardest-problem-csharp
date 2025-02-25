public class Solution
{
    public bool CanReachCorner(int xCorner, int yCorner, int[][] circles)
    {
        var question = new Question(
            new Area(xCorner, yCorner),
            circles.Select(c => new Circle(c[0], c[1], c[2])).ToList());

        return Explore(question) == null;
    }

    public static Path? Explore(Question question)
    {
        var startPaths = new List<Path>();
        var endPaths = new List<Path>();
        var currentCircles = new Queue<Circle>();

        foreach (var circle in question.Circles)
        {
            if (PointCovering(0, 0, circle)
                || PointCovering(question.Area.Width, question.Area.Height, circle))
            {
                return new Path(new List<Circle> { circle });
            }

            var start = IntersectingY(circle, 0, question.Area)
                || IntersectingX(circle, question.Area.Width, question.Area);
            if (start)
            {
                startPaths.Add(new Path(new List<Circle> { circle }));
            }

            var end = IntersectingY(circle, question.Area.Height, question.Area)
                || IntersectingX(circle, 0, question.Area);
            if (end)
            {
                endPaths.Add(new Path(new List<Circle> { circle }));
            }

            if (start && end)
            {
                return new Path(new List<Circle> { circle });
            }

            if (!start && !end)
            {
                currentCircles.Enqueue(circle);
            }
        }

        foreach (var startPath in startPaths)
        {
            var startCircle = startPath.Circles.Last();
            foreach (var endPath in endPaths)
            {
                var endCircle = endPath.Circles.Last();
                if (IntersectingCircle(startCircle, endCircle, question.Area))
                {
                    return new Path(new List<Circle> { startCircle, endCircle });
                }
            }
        }

        while (currentCircles.Count > 0)
        {
            var currentCircle = currentCircles.Dequeue();

            Path? targetPath = null;

            foreach (var startPath in startPaths)
            {
                var pathCircle = startPath.Circles.Last();
                if (IntersectingCircle(currentCircle, pathCircle, question.Area))
                {
                    startPath.Circles.Add(currentCircle);
                    targetPath = startPath;
                }
            }

            foreach (var endPath in endPaths)
            {
                var pathCircle = endPath.Circles.Last();
                if (IntersectingCircle(currentCircle, pathCircle, question.Area))
                {
                    if (targetPath != null)
                    {
                        return new Path(
                            targetPath.Circles
                                .Concat(((IEnumerable<Circle>)endPath.Circles)
                                .Reverse()).ToList()
                        );
                    }

                    endPath.Circles.Add(currentCircle);
                }
            }
        }

        return null;
    }

    public static bool IntersectingCircle(Circle circle1, Circle circle2, Area area)
    {
        long dx = circle1.X - circle2.X;
        long dy = circle1.Y - circle2.Y;
        long radiusSum = circle1.Radius + circle2.Radius;

        if (dx * dx + dy * dy > radiusSum * radiusSum)
        {
            return false;
        }

        var midpointX = ((long)circle1.X * circle2.Radius + (long)circle2.X * circle1.Radius) / (double)radiusSum;
        var midpointY = ((long)circle1.Y * circle2.Radius + (long)circle2.Y * circle1.Radius) / (double)radiusSum;

        return midpointX >= 0 && midpointX <= area.Width
            && midpointY >= 0 && midpointY <= area.Height;
    }

    public static bool IntersectingX(Circle circle, int x, Area area)
    {
        var dx = circle.X - x;
        if (Math.Abs(dx) <= circle.Radius)
        {
            var xy = Math.Sqrt((long)circle.Radius * circle.Radius - (long)dx * dx);
            double y1 = circle.Y - xy;
            double y2 = circle.Y + xy;

            return (0 < y1 && y1 < area.Height)
                || (0 < y2 && y2 < area.Height);
        }

        return false;
    }

    public static bool IntersectingY(Circle circle, int y, Area area)
    {
        var dy = circle.Y - y;
        if (Math.Abs(dy) <= circle.Radius)
        {
            double yx = Math.Sqrt((long)circle.Radius * circle.Radius - (long)dy * dy);
            double x1 = circle.X - yx;
            double x2 = circle.X + yx;

            return (0 < x1 && x1 < area.Width)
                || (0 < x2 && x2 < area.Width);
        }

        return false;
    }

    public static bool PointCovering(int x, int y, Circle circle)
    {
        long dx = x - circle.X;
        long dy = y - circle.Y;

        return dx * dx + dy * dy <= (long)circle.Radius * circle.Radius;
    }
}

public record class Question(Area Area, List<Circle> Circles);
public record Area(int Width, int Height);
public record Circle(int X, int Y, int Radius);

public record Path(List<Circle> Circles);
