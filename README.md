# LeetCode's Hardest Problem

This problem is rated as the **top most difficult question** on [LeetCode](https://leetcode.com/problems/check-if-the-rectangle-corner-is-reachable) by [zerotrac's rating](https://zerotrac.github.io/leetcode_problem_rating/).

![Rating Table](https://github.com/tommyinb/leetcode-hardest-problem/raw/master/src/tutorials/rating-table.png)

Very few people have successfully solved this problem.

![Submit](https://github.com/tommyinb/leetcode-hardest-problem/raw/master/preview/submit.png)

## Interactive Page

If you want to learn more, I've created a interactive page for you. ✨

<https://tommyinb.github.io/leetcode-hardest-problem/>

## Check if the Rectangle Corner is Reachable

Frankly speaking, this question is straightforward. It is considered reachable if we can go from the top-left to the bottom-right. If there is a circle in our way, we just go around it.

If we loop back to the origin, we can conclude that it is unreachable.

![Reachability](https://github.com/tommyinb/leetcode-hardest-problem/raw/master/preview/reachability.png)

## Geometry is Tricky

Although it looks easy, implementation is not an easy task. Therefore, most people, including the question's author, resort to a wrong approach.

![Wrong Solution](https://github.com/tommyinb/leetcode-hardest-problem/raw/master/src/tutorials/wrong-header.png)
![Comment](https://github.com/tommyinb/leetcode-hardest-problem/raw/master/src/tutorials/wrong-comment.png)

Sadly, all solutions posted by the community wrongly use topology instead of geometry, and use circle centers instead of circumferences.

![Wrong Method](https://github.com/tommyinb/leetcode-hardest-problem/raw/master/preview/wrong-method.png)

This is understandable because using points instead of lines is just one dimension less complex. This convenience blinds people.

![Test Case](https://github.com/tommyinb/leetcode-hardest-problem/raw/master/preview/wrong-case.png)

In the above case of only two circles, they enter the rectangle and connect to each other, but they also exit the rectangle right before the corner, leaving the corner reachable. Unless we zoom in 100 times, our eyes can never see this tricky geometry.

## Code It

Go right, then go down, and go around the circles. ([Solution.cs](./Solution.cs))

```csharp
private static IStep? MoveStep(IStep currentStep, Question question)
    => currentStep switch
    {
        RightLineStep rightLineStep =>
            MoveRight(rightLineStep, question),

        DownLineStep downLineStep =>
            MoveDown(downLineStep, question),

        ArcStep arcStep =>
            MoveArc(arcStep, question),

        _ => throw new InvalidOperationException(),
    };
```

## Deal with Geometry

"Go around the circles" is intuitive for humans but challenging to implement in code. Without visualizing it on a chart, we are left with just a series of intersection points.

```csharp
private static IEnumerable<ArcNext> GetArcNexts(
    ArcStep currentStep, Question question)
{
    var circles = question.Circles
        .Except(new[] { currentStep.Circle });

    return circles.SelectMany(circle =>
        GetCircleIntersects(currentStep.Circle, circle)
        .Where(intersect =>
            intersect.X > 0
            && intersect.X < question.Area.Width
            && intersect.Y > 0
            && intersect.Y < question.Area.Height)
        .Select(intersect => new ArcNext(
            Math.Atan2(
                intersect.Y - currentStep.Circle.Y,
                intersect.X - currentStep.Circle.X),
            new ArcStep(intersect.X, intersect.Y, circle))));
}
```

Equation of locus of circle is taught in school. But they didn't tell us one important thing - sweeping is directional. Therefore, finding the intersection points is not enough. We need to order by angles and take the counter-clockwise turn.

```csharp
const currentAngle = Math.atan2(
  currentStep.y - currentStep.circle.y,
  currentStep.x - currentStep.circle.x
);

return nexts
    .OrderBy(next => next.Angle - currentAngle)
    .Select(next => next.Step)
    .LastOrDefault();
```

## Interactive Page ❤️

If you want to learn more, I've created a interactive page for you.

<https://tommyinb.github.io/leetcode-hardest-problem/>

Happy coding! 😊
