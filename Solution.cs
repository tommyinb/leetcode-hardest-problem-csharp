public class Solution
{
    public bool CanReachCorner(int xCorner, int yCorner, int[][] circles)
    {
        var question = new Question(
            new Area(xCorner, yCorner),
            circles.Select(c => new Circle(c[0], c[1], c[2])).ToArray());

        var abc = Covering(question);

        return !Covering(question)
            && Explore(question);
    }

    private static bool Covering(Question question)
        => question.Circles.Where(c =>
            ((long)c.X * c.X + (long)c.Y * c.Y
                <= (long)c.Radius * c.Radius)
            || (((long)c.X - question.Area.Width) * ((long)c.X - question.Area.Width)
                + ((long)c.Y - question.Area.Height) * ((long)c.Y - question.Area.Height)
                <= (long)c.Radius * c.Radius))
            .Any();

    private static bool Explore(Question question)
    {
        List<IStep> steps = new();
        IStep currentStep = new RightLineStep(0);

        for (var i = 0; i < question.Circles.Length * 3 + 50; i++)
        {
            var nextStep = MoveStep(currentStep, question);

            switch (nextStep)
            {
                case null: return false;
                case CornerEndStep _: return true;
                case LeftEndStep _:
                case BottomEndStep _: return false;
            }

            steps.Add(currentStep);
            currentStep = nextStep;
        }

        return false;
    }
    private static IStep? MoveStep(IStep currentStep, Question question)
        => currentStep switch
        {
            RightLineStep rightLineStep => MoveRight(rightLineStep, question),
            DownLineStep downLineStep => MoveDown(downLineStep, question),
            ArcStep arcStep => MoveArc(arcStep, question),
            _ => throw new InvalidOperationException(),
        };

    private static IStep MoveRight(RightLineStep currentStep, Question question)
    {
        var arcStep = question.Circles
            .SelectMany(circle =>
            {
                var delta = (long)circle.Radius * circle.Radius
                    - (long)circle.Y * circle.Y;

                if (delta < 0)
                {
                    return Array.Empty<ArcStep>();
                }

                var dx = Math.Sqrt(delta);
                var xs = new[] { circle.X - dx, circle.X + dx };

                return xs.Select(x => new ArcStep(x, 0, circle));
            })
            .Where(arcStep => arcStep.X > currentStep.X)
            .Where(arcStep => arcStep.X < question.Area.Width)
            .OrderBy(arcStep => arcStep.X)
            .FirstOrDefault();

        if (arcStep != null)
        {
            return arcStep;
        }

        return new DownLineStep(0);
    }
    private static IStep MoveDown(DownLineStep currentStep, Question question)
    {
        var arcStep = question.Circles
            .SelectMany(circle =>
            {
                var delta = (long)circle.Radius * circle.Radius
                    - (long)(question.Area.Width - circle.X) * (question.Area.Width - circle.X);

                if (delta < 0)
                {
                    return Array.Empty<ArcStep>();
                }

                var dy = Math.Sqrt(delta);
                var ys = new[] { circle.Y - dy, circle.Y + dy };

                return ys.Select(y => new ArcStep(question.Area.Width, y, circle));
            })
            .Where(arcStep => arcStep.Y > currentStep.Y)
            .Where(arcStep => arcStep.Y < question.Area.Height)
            .OrderBy(arcStep => arcStep.Y)
            .FirstOrDefault();

        if (arcStep != null)
        {
            return arcStep;
        }

        return new CornerEndStep();
    }

    private static IStep? MoveArc(ArcStep currentStep, Question question)
    {
        var shit = Math.Atan2(
            currentStep.Y - currentStep.Circle.Y,
            currentStep.X - currentStep.Circle.X);

        var currentAngle = shit;

        while (currentAngle >= Math.PI * 2)
        {
            currentAngle -= Math.PI * 2;
        }
        while (currentAngle < 0)
        {
            currentAngle += Math.PI * 2;
        }

        var nexts = GetRightNexts(currentStep, question)
            .Concat(GetDownNexts(currentStep, question))
            .Concat(GetLeftNexts(currentStep, question))
            .Concat(GetBottomNexts(currentStep, question))
            .Concat(GetArcNexts(currentStep, question))
            .ToArray();

        var nexts2 = nexts
            .Select(next =>
            {
                var angle = next.Angle;

                while (angle >= Math.PI * 2)
                {
                    angle -= Math.PI * 2;
                }
                while (angle < 0)
                {
                    angle += Math.PI * 2;
                }

                angle -= currentAngle;

                while (angle >= Math.PI * 2)
                {
                    angle -= Math.PI * 2;
                }
                while (angle < -0.000001)
                {
                    angle += Math.PI * 2;
                }

                angle += Random.Shared.NextDouble() * 0.000001;

                return new ArcNext(angle, next.Step);
            })
            .ToArray();

        return nexts2
            .OrderBy(next => next.Angle)
            .Select(next => next.Step)
            .LastOrDefault();
    }

    private static IEnumerable<ArcNext> GetRightNexts(ArcStep currentStep, Question question)
        => GetXIntersects(currentStep, 0, question)
            .Select(xIntersect => new ArcNext(xIntersect.Angle, new RightLineStep(xIntersect.X)));
    private static IEnumerable<ArcXIntersect> GetXIntersects(ArcStep currentStep, double y, Question question)
    {
        var delta = (long)currentStep.Circle.Radius * currentStep.Circle.Radius
            - (long)(y - currentStep.Circle.Y) * (y - currentStep.Circle.Y);

        if (delta < 0)
        {
            return Array.Empty<ArcXIntersect>();
        }

        var dx = Math.Sqrt(delta);
        var xs = new[] {
            currentStep.Circle.X + dx,
            currentStep.Circle.X - dx
        };

        return xs
            .Where(x => x >= 0)
            .Where(x => x <= question.Area.Width)
            .Select(x => new ArcXIntersect(x,
                Math.Atan2(y - currentStep.Circle.Y, x - currentStep.Circle.X)));
    }
    private static IEnumerable<ArcNext> GetDownNexts(ArcStep currentStep, Question question)
        => GetYIntersects(currentStep, question.Area.Width, question)
            .Select(yIntersect => new ArcNext(yIntersect.Angle, new DownLineStep(yIntersect.Y)));
    private static IEnumerable<ArcYIntersect> GetYIntersects(ArcStep currentStep, double x, Question question)
    {
        var delta = (long)currentStep.Circle.Radius * currentStep.Circle.Radius
            - (long)(x - currentStep.Circle.X) * (x - currentStep.Circle.X);

        if (delta < 0)
        {
            return Array.Empty<ArcYIntersect>();
        }

        var dy = Math.Sqrt(delta);
        var ys = new[] {
            currentStep.Circle.Y + dy,
            currentStep.Circle.Y - dy
        };

        return ys
            .Where(y => y >= 0)
            .Where(y => y <= question.Area.Height)
            .Select(y => new ArcYIntersect(y,
                Math.Atan2(y - currentStep.Circle.Y, x - currentStep.Circle.X)));
    }
    private static IEnumerable<ArcNext> GetLeftNexts(ArcStep currentStep, Question question)
        => GetYIntersects(currentStep, 0, question)
            .Select(intersect => new ArcNext(intersect.Angle, new LeftEndStep(intersect.Y)));
    private static IEnumerable<ArcNext> GetBottomNexts(ArcStep currentStep, Question question)
        => GetXIntersects(currentStep, question.Area.Height, question)
            .Select(intersect => new ArcNext(intersect.Angle, new BottomEndStep(intersect.X)));

    private static IEnumerable<ArcNext> GetArcNexts(ArcStep currentStep, Question question)
    {
        var circles = question.Circles.Except(new[] { currentStep.Circle });

        return circles.SelectMany(circle =>
            GetCircleIntersects(currentStep.Circle, circle)
            .Where(intersect =>
                intersect.X > 0 && intersect.X < question.Area.Width
                && intersect.Y > 0 && intersect.Y < question.Area.Height)
            .Select(intersect => new ArcNext(
                Math.Atan2(intersect.Y - currentStep.Circle.Y, intersect.X - currentStep.Circle.X),
                new ArcStep(intersect.X, intersect.Y, circle))));
    }
    private static IEnumerable<CircleIntersect> GetCircleIntersects(Circle a, Circle b)
    {
        long dx = b.X - a.X;
        long dy = b.Y - a.Y;
        var d = Math.Sqrt(dx * dx + dy * dy);

        if (d > a.Radius + b.Radius
            || d < Math.Abs(a.Radius - b.Radius))
        {
            yield break;
        }

        var a2 = ((long)a.Radius * a.Radius - (long)b.Radius * b.Radius + d * d) / (2 * d);
        var h = Math.Sqrt((long)a.Radius * a.Radius - a2 * a2);

        var xm = a.X + a2 * dx / d;
        var ym = a.Y + a2 * dy / d;

        yield return new CircleIntersect(
            xm + h * dy / d,
            ym - h * dx / d);

        yield return new CircleIntersect(
            xm - h * dy / d,
            ym + h * dx / d);
    }
}

record Question(Area Area, Circle[] Circles);
record Area(int Width, int Height);
record Circle(int X, int Y, int Radius);

interface IStep { }
record RightLineStep(double X) : IStep;
record DownLineStep(double Y) : IStep;
record ArcStep(double X, double Y, Circle Circle) : IStep;
record CornerEndStep() : IStep;
record LeftEndStep(double Y) : IStep;
record BottomEndStep(double X) : IStep;

record ArcNext(double Angle, IStep Step);
record ArcXIntersect(double X, double Angle);
record ArcYIntersect(double Y, double Angle);
record CircleIntersect(double X, double Y);