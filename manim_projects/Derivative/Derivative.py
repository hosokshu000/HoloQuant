from manim import *
import math

config.pixel_width = 1920
config.pixel_height = 1920
config.frame_rate = 60

class Derivative(MovingCameraScene):
    def construct(self):
        # Set up axes
        axes = Axes(
            x_range=[-5, 5],
            y_range=[-5, 5],
            x_length=12,
            y_length=12,
            axis_config={"include_numbers": True, "tip_shape": StealthTip}
        )

        # Define the function
        def func(x):
            return x*1/2

        # Create the graph
        graph = axes.plot(func, color=BLUE)

        label = MathTex("f(x) = \\frac{1}{2}x", color=WHITE, font_size=36)

        # Create a label for the function
        graph_label = axes.get_graph_label(graph, label=label, color=WHITE).move_to([6, 6, 0])

        self.camera.frame.save_state()

        # Create the graph
        self.play(Create(axes), run_time=2)
        self.play(Create(graph), Write(graph_label), run_time=2)
        self.wait(1)


        # Zoom in on the graph
        self.play(self.camera.frame.animate.set(width=8).move_to([3, 3, 0]),run_time=2)
        self.wait(1)


        # Draw two points on the graph
        p1 = Dot(axes.c2p(2, 1), color=YELLOW)
        p2 = Dot(axes.c2p(4, 2), color=YELLOW)

        self.play(Create(p1), Create(p2), run_time=1)
        self.wait(1)


        # Draw dashed lines and take the delta of each axis
        yDash = DashedLine(axes.c2p(4, 1), axes.c2p(4, 2), color=WHITE)
        yDash.z_index = -1
        xDash = DashedLine(axes.c2p(2, 1), axes.c2p(4, 1), color=WHITE)
        xDash.z_index = -1
        self.play(Create(yDash), Create(xDash), run_time=1)    

        yBrace = BraceBetweenPoints(axes.c2p(4, 1),axes.c2p(4, 2))
        xBrace = BraceBetweenPoints(axes.c2p(2, 1), axes.c2p(4, 1))        
        yLabel = MathTex(r"\Delta y", color=WHITE, font_size=36).next_to(yBrace, RIGHT + LEFT * 0.5)
        xLabel = MathTex(r"\Delta x", color=WHITE, font_size=36).next_to(xBrace, DOWN + UP * 0.5)
        self.play(Create(yBrace), Create(xBrace), Create(yLabel), Create(xLabel), run_time=1)
        self.wait(1)


        # Demonstrate the slope using the two points
        slopeLabel = MathTex(r"\text{Slope} = \frac{\Delta y}{\Delta x}", font_size=36).move_to([2, 4, 0])
        self.play(Write(slopeLabel), run_time=1)
        self.wait(1)

        slopeSubstitute = MathTex(r" = \frac{1}{2}", font_size=36).move_to(slopeLabel.get_right() + RIGHT * 0.5)
        slopeSubstitute[0][1].set_opacity(0)
        slopeSubstitute[0][3].set_opacity(0)

        numerator = MathTex("1", color=WHITE, font_size=36).move_to(slopeSubstitute[0][1])
        denominator = MathTex("2", color=WHITE, font_size=36).move_to(slopeSubstitute[0][3])

        self.play(Write(slopeSubstitute), run_time=1)
        self.play(Transform(yLabel,numerator), Transform(xLabel, denominator), run_time=1.5)
        self.wait(1)


        # Clear the linear graph and draw a parabola
        self.play(Restore(self.camera.frame),
                  *[FadeOut(mob) for mob in self.mobjects if mob != axes and mob != self.camera.frame],
                  run_time=1)
        
        def parabola(x):
            return 0.5 * x**2
        
        parabolaGraph = axes.plot(parabola, color=BLUE)
        parabolaLabel = MathTex("f(x) = 0.5x^2", color=WHITE, font_size=36)
        parabolaGraphLabel = axes.get_graph_label(parabolaGraph, label=parabolaLabel, color=WHITE).move_to([6, 6, 0])

        self.play(Create(parabolaGraph), Write(parabolaGraphLabel), run_time=2)
        self.play(self.camera.frame.animate.set(width=8).move_to([3, 3, 0]),run_time=2)
        self.wait(1)


        alpha_tracker = ValueTracker(0.5)

        tangent = always_redraw(lambda: TangentLine(
            parabolaGraph,
            alpha=alpha_tracker.get_value(),
            length=4,
            color=YELLOW
        ))

        dot = always_redraw(lambda: Dot(
            parabolaGraph.point_from_proportion(alpha_tracker.get_value()),
            color=RED,
            z_index=1
        ))

        self.play(Create(tangent), Create(dot))
        self.play(alpha_tracker.animate.set_value(0.8), run_time=1, rate_func=linear)
        self.wait(1)
