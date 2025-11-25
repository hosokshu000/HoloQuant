from manim import *


class CreateCircle(Scene):
    def construct(self):
        self.camera.background_color = None
        circle = Circle()  # create a circle
        circle.set_fill(PINK, opacity=0.5)  # set the color and transparency

        square = Square()
        square.rotate(PI / 4)
        self.play(Create(square))
        self.play(Transform(square, circle))  # show the circle on screen
        self.play(FadeOut(square))