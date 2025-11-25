from manim import *
import math

class circleArea(Scene):
    def construct(self):
        RADIUS_COLOR = DARK_BLUE
        CIRCUMFERENCE_COLOR = DARK_BLUE
        CIRCLE_FILL = BLUE_B
        global originalSectors
        global brace1, brace2, brace_label1, brace_label2

        sector = Sector()
        arc = Arc()
    
        # Custom transform function that also transforms the position and rotation of the Mobject
        def custom_transform(mob, alpha, initial_obj: VGroup, target_obj: VGroup):
            # Create a new VGroup for the interpolated state
            new_mob = VGroup()
            
            # Interpolate each submobject separately
            for init_submob, target_submob in zip(initial_obj, target_obj):
                new_submob = init_submob.copy()
                new_submob.interpolate(init_submob, target_submob, alpha)
                new_mob.add(new_submob)

            start_pos = initial_obj.get_center().copy()
            end_pos = target_obj.get_center().copy()
            
            # Interpolate position
            new_pos = start_pos * (1 - alpha) + end_pos * alpha
            new_mob.move_to(new_pos)

            if len(new_mob) > 0:
                start_angle = initial_obj[0].get_angle()
                end_angle = target_obj[0].get_angle()
                new_angle = start_angle * (1 - alpha) + end_angle * alpha
                new_mob.rotate(new_angle - new_mob[0].get_angle())
            
            mob.become(new_mob)

        # Line rotates and fans out into a circle
        def initiateCircle(mob, alpha):
            # Set the sector's angle proportional to the alpha value (0 to 2*PI)
            sector.become(Sector(outer_radius=2, angle=alpha * 2 * PI, fill_color=CIRCLE_FILL, fill_opacity=1, stroke_width=0, z_index=0))
            arc.become(Arc(radius=2, angle=alpha * 2 * PI, stroke_color=CIRCUMFERENCE_COLOR, stroke_width=6, fill_opacity=0, z_index=1))
        
        # Draw slice lines from the radius of the circle to the circumference of the circle
        def sliceLines(numSlices: int):
            lines = [] # List of slice lines
            length = 2 # Circle's radius (Length of slice line)
            dAngle = 2 * PI / numSlices # Angle increment for slices
            # Iterate for numSlices / 2 as slice lines are diameters (every slice creates twice as many sectors as the previous)
            for l in range(numSlices):
                start_point = ORIGIN
                end_point = [length * np.cos(l * dAngle), length * np.sin(l * dAngle), 0] # Pythagorean theorem
                lines.append(Line(start_point, end_point, stroke_color=RADIUS_COLOR, stroke_width=1, z_index=1))
            return lines

        # Create equal sectors to "slice" the circle into
        def createSectors(numSlices: int):
            sectors = VGroup() # Create VGroup for pulling all sectors away from the center
            length = 2 # Circle's radius
            dAngle = 2 * PI / numSlices # Angle increment for sector rotation
            # Create sectors and rotate them accordingly so they form their corresponding segment of the circle
            for numArc in range(numSlices):
                sector = Sector(outer_radius=length, angle = dAngle, fill_color=CIRCLE_FILL, fill_opacity=1, z_index=0, stroke_width=0)
                arc = Arc(radius=2, start_angle=numArc * dAngle, angle=dAngle, stroke_color=CIRCUMFERENCE_COLOR, stroke_width=6, fill_opacity=0, z_index=1)
                sector.rotate(numArc * dAngle, about_point=ORIGIN)
                vgroup = VGroup(sector, arc)
                sectors.add(vgroup)
            return sectors
        
        # Disperse the sectors outwards from the center of the circle
        def disperseSectors(sectors: List[Mobject], dist: int):
            animations = [] # List for each sector shift animation
            sectCnt = 0 # Counter for the next loop
            dAngle = 2 * PI / len(sectors) # Angle increment for each sector transform vector

            # Create a list of transformations for each sector
            for sector in sectors:
                # Transformation vector for each corresponding sector
                shift_vector = [dist * np.cos(dAngle / 2 + dAngle * sectCnt), dist * np.sin(dAngle / 2 + dAngle * sectCnt), 0]
                animation = ApplyMethod(sector.shift, shift_vector)
                animations.append(animation)
                sectCnt += 1
            return animations

        # Move the sectors into a parallelogram
        def rearrangeSectors(sectors: List[Mobject], numSectors: int):
            global brace1, brace2, brace_label1, brace_label2

            sector = Sector(outer_radius=2, start_angle =- PI / numSectors, angle =2 * PI / numSectors, fill_color=CIRCLE_FILL, fill_opacity=1, z_index=0, stroke_width=0)
            arc = Arc(radius=2, start_angle =- PI / numSectors, angle=2 * PI / numSectors, stroke_color=CIRCUMFERENCE_COLOR, stroke_width=6, fill_opacity=0, z_index=1)

            # Define base sector
            new_sector = VGroup(sector, arc)

            new_sectors = [] # Store finalized sectors
            horizontal_offSet = 2 * np.sin(PI / numSectors) # Horizontal offset distance for each new sector
            
            center_pos = [] # Desired final position
            midpoint_pos = [] # Position at midpoint of the animation

            for i in range(-numSectors // 2, numSectors // 2, 2):
                if(numSectors < 16):
                    center_pos.append([horizontal_offSet / 2 + horizontal_offSet * i, 0.06, 0])
                else:
                    center_pos.append([horizontal_offSet / 2 + horizontal_offSet * i, 0, 0])
                midpoint_pos.append([horizontal_offSet / 2 + horizontal_offSet * i, 1, 0]) # At midpoint, sectors "line up" parallel to each other

            for i in range(-numSectors // 2 + 1, numSectors // 2, 2):
                if(numSectors < 16):
                    center_pos.append([horizontal_offSet / 2 + horizontal_offSet * i, -0.06, 0])
                else:
                    center_pos.append([horizontal_offSet / 2 + horizontal_offSet * i, 0, 0])
                midpoint_pos.append([horizontal_offSet / 2 + horizontal_offSet * i, -1, 0])

            midpoint_sectors = [] # Store sectors at intermediate step

            for i in range(len(midpoint_pos)):
                sector_copy = new_sector.copy()
                sector_copy.move_to(midpoint_pos[i])

                # Alternate the rotation between PI / 2 and - PI / 2
                if i < numSectors // 2:
                    scaleFactor = 1
                else:
                    scaleFactor = -1
                
                sector_copy.rotate(PI / 2 * scaleFactor, about_point=sector_copy.get_center())
                midpoint_sectors.append(sector_copy)

            animation1 = [] # Store first phase of the animation
            animation2 = [] # Store second phase of the animation
            animation3 = [] # Store final phase of the animation

            for i in range(len(midpoint_sectors)):
                animation = UpdateFromAlphaFunc(
                    sectors[i],
                    lambda mob, alpha, i=i: custom_transform(
                        mob,
                        alpha,
                        sectors[i],
                        midpoint_sectors[i]
                    ), 
                    run_time=1
                )
                animation1.append(animation)
            
            # Populate new_sectors with the desired position and rotation of each sector after moving
            for i in range(len(center_pos)):
                sector_copy = new_sector.copy()
                sector_copy.move_to(center_pos[i])

                # Alternate the rotation between PI / 2 and - PI / 2
                if i < numSectors // 2:
                    scaleFactor = 1
                else:
                    scaleFactor = -1
                
                sector_copy.rotate(PI / 2 * scaleFactor, about_point=sector_copy.get_center())
                new_sectors.append(sector_copy)


            for i in range(len(new_sectors)):
                animation = UpdateFromAlphaFunc(
                    sectors[i],
                    lambda mob, alpha, i=i: custom_transform(
                        mob,
                        alpha,
                        sectors[i],
                        new_sectors[i]
                    ),
                    run_time=2
                )
                animation2.append(animation)

            start_pos = new_sectors[0].get_center() + [0, -1.5, 0]
            lineLength = 4 * PI / numSectors # Each line segment is the arc length of each sector
            lines = []

            originalArcs = [] # Store original arcs to revert back to

            arcToLine = []

            for i in range(numSectors // 2):
                lines.append(Line(start_pos, start_pos + [lineLength, 0, 0], stroke_color=CIRCUMFERENCE_COLOR, stroke_width=6))
                start_pos += [lineLength, 0, 0]                

            for i in range(len(new_sectors) // 2, len(new_sectors)):
                arc_copy = new_sectors[i][1].copy()
                originalArcs.append(arc_copy)
                animation = Transform(sectors[i][1], lines[i - len(new_sectors) // 2])
                arcToLine.append(animation)

            brace1 = Brace(new_sectors[0], direction=LEFT)
            brace1.move_to(new_sectors[0].get_center() + [-math.sin(PI / numSectors) * math.cos(PI / numSectors) - 0.3, -pow(math.sin(PI / numSectors), 2), 0])
            
            if numSectors != 256:
                brace1.rotate(PI / numSectors)

            brace_label1 = MathTex("r").next_to(brace1, LEFT)

            brace2 = Brace(VGroup(*lines), direction=DOWN)
            brace_label2 = MathTex("\\pi r").next_to(brace2, DOWN)

            writingAnimation = []

            writingAnimation.extend([Create(brace1), Write(brace_label1)])
            writingAnimation.extend([Create(brace2), Write(brace_label2)])

            lineToArc = []

            for i in range(len(lines)):
                animation = Transform(sectors[i + len(sectors) // 2][1], originalArcs[i])
                lineToArc.append(animation)
            
            # Return back to parallel sectors
            for i in range(len(midpoint_sectors)):
                animation = UpdateFromAlphaFunc(
                    sectors[i],
                    lambda mob, alpha, i=i: custom_transform(
                        mob,
                        alpha,
                        sectors[i],
                        midpoint_sectors[i]
                    ), 
                    run_time=1
                )
                animation3.append(animation)

            animation3.extend([FadeOut(brace1, run_time=0.1), Unwrite(brace_label1, run_time=0.1)])
            animation3.extend([FadeOut(brace2, run_time=0.1), Unwrite(brace_label2, run_time=0.1)])
            
            return animation1, animation2, animation3, writingAnimation, arcToLine, lineToArc

        # Consolidate sectors back into a circle
        def sectorsToCircle(sectors: List[Mobject]):
            animations = []
            for index in range(len(sectors)):
                animation = UpdateFromAlphaFunc(
                    sectors[index], 
                    lambda mob, alpha, i=index: custom_transform(
                        mob, 
                        alpha, 
                        initial_obj=sectors[i], 
                        target_obj=originalSectors[i]
                    )
                )
                animations.append(animation)
            return animations

        


        # 0[Make]
        # Animate the rotation of the radius line and update the sector fill
        radius = Line([0, 0, 0], [2, 0, 0], stroke_color=CIRCLE_FILL, stroke_width=6)        
        self.play(Create(radius), run_time=0.6)
        self.play(LaggedStart(AnimationGroup(UpdateFromAlphaFunc(sector, initiateCircle), UpdateFromAlphaFunc(arc, initiateCircle)), FadeOut(radius, run_time=1), lag_ratio=0.1))
        
        self.remove(radius)

        areaLabel = MathTex("\\pi r^2", font_size=48, color=WHITE).move_to([0, 0, 0]) # Create the label for the area of the circle

        self.play(Write(areaLabel))
        self.wait(1)
        self.play(Unwrite(areaLabel))

        self.wait(1)


        # 1[Define]
        pi_approx = MathTex("\\pi \\approx 3.14159\\ldots").move_to([-3, 3, 0])

        self.play(Write(pi_approx))

        self.wait(1)


        # 2[Demonstrate]
        radius = Line([0, 0, 0], [2, 0, 0], stroke_color=RADIUS_COLOR, stroke_width=6) # Create the radius again with different color
        radiusLabel = MathTex("r", font_size=48, color=WHITE).next_to(radius, UP) # Position the radius label

        self.play(Create(radius))
        self.play(Write(radiusLabel))

        circumference = Line([-2 * PI, -2, 0], [2 * PI, -2, 0], stroke_color=CIRCUMFERENCE_COLOR, stroke_width=6, z_index=1) # Create the straight line for the circumference to "roll out" into
        group = VGroup(sector, radiusLabel, radius) # Group the mobjects to move up

        self.play(ReplacementTransform(arc, circumference), group.animate.shift([0, 1, 0])) # Move the circle (group) up while "unwrapping" the circumference

        circumLabel = MathTex("2\\pi r", font_size=48, color=WHITE).next_to(circumference, DOWN) # Create label for the circumference line

        self.play(Write(circumLabel))

        self.wait(1)


        # 3[Return]
        self.play(Unwrite(circumLabel), Unwrite(radiusLabel), Unwrite(pi_approx), Uncreate(radius))

        arc = Arc(radius=2, angle=2 * PI, stroke_color=CIRCUMFERENCE_COLOR, stroke_width=6, fill_opacity=0, z_index=1) # Redefine arc (to reset the transform)

        self.play(ReplacementTransform(circumference, arc), group.animate.shift([0, -1, 0]))
        self.wait(1)

        
        # Slicing
        sliceOrder = [8, 16, 64, 256] # Order of slices created
        
        for slices in sliceOrder:
            sliceLabel = Text(f"Slices: {slices}").move_to([-4, 3, 0])

            self.play(Write(sliceLabel))

            sectors = createSectors(slices) # List of sectors for the circle to be sliced into
            originalSectors = VGroup(*[sector.copy() for sector in sectors])
            lines = sliceLines(slices) # List of lines to denote each slice

            for line in lines:
                self.play(Create(line), run_time=0.8 / slices)

            for sector in sectors:
                self.add(sector)
            
            self.remove(*[obj for obj in self.mobjects if obj != sliceLabel]) # Remove the circle after the slice lines and sectors are added (so the removal is unnoticable)

            fadeoutAnimations = [FadeOut(line, run_time=0.5) for line in lines]
            sectorAnimations = disperseSectors(sectors, 0.5)

            self.play(LaggedStart(AnimationGroup(*sectorAnimations), AnimationGroup(*fadeoutAnimations), lag_ratio=0.1))
            self.wait(1)

            rearrange1, rearrange2, rearrange3, writingAnimation, a2l, l2a = rearrangeSectors(sectors, slices)
            self.play(*rearrange1)

            #self.play(LaggedStart(AnimationGroup(*rearrange2), AnimationGroup(*writingAnimation), lag_ratio=0.2))
            self.play(*rearrange2)
            self.play(*a2l)
            self.play(*writingAnimation)

            self.wait(1)

            self.play(*l2a)

            sector = Sector(outer_radius=2, angle=2 * PI, fill_color=CIRCLE_FILL, fill_opacity=1, stroke_width=0, z_index=0)


            # Approx to Rectangle
            if slices == 256:
                rectangle = Rectangle(height=2, width=2 * PI, fill_color=CIRCLE_FILL, fill_opacity=1, stroke_width=0)
                line1 = Line([-PI, 1, 0], [PI, 1, 0], stroke_color=CIRCUMFERENCE_COLOR, stroke_width=6, fill_opacity=0, z_index=1)
                line2 = Line([-PI, -1, 0], [PI, -1, 0], stroke_color=CIRCUMFERENCE_COLOR, stroke_width=6, fill_opacity=0, z_index=1).move_to([0, -1, 0])

                self.remove(*sectors)
                self.add(rectangle, line1, line2)
            else:
                self.wait(1)
                self.play(*rearrange3)
                returnAnimation = sectorsToCircle(sectors)
                self.play(*returnAnimation)
                self.remove(*sectors)
                self.add(arc, sector)
            
            self.play(Unwrite(sliceLabel))

            self.wait(1)


        # Rectangle Area Definition
        objects = Group(*self.mobjects)
        
        rectArea = MathTex("Area & = Height \\times Width\\\\", "& = ", "r", " \\times ", "\\pi r\\\\", "& = ", "\\pi r^2").move_to([-3, 2, 0])
        self.play(objects.animate.shift([0, -1, 0]), Write(rectArea[0:2]))
        self.play(Write(rectArea[3]), brace_label1.animate.move_to(rectArea[2].get_center()), brace_label2.animate.move_to(rectArea[4].get_center()), FadeOut(brace1), FadeOut(brace2))
        self.play(Write(rectArea[5:]))

        self.wait(1)


        # Move Area
        self.play(rectArea[6].animate.move_to(rectangle.get_center()))
        self.wait(1)


        # Return to Circle
        self.play(
            Unwrite(rectArea[:6], run_time=0.6),
            Unwrite(brace_label1),
            Unwrite(brace_label2),
            Transform(line1, arc),
            Transform(rectangle, sector),
            FadeOut(line2, run_time=math.ulp(0.0)),
            rectArea[6].animate.move_to([0, 0, 1])
        )

        self.wait(1)