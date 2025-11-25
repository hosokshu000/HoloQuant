from manim import *

class SimpleLatexExample(Scene):
    def construct(self):
        # Simple LaTeX expression
        label = MathTex(r"2 \pi r", font_size=48, color=WHITE)
        self.play(Write(label))
        self.wait(2)
