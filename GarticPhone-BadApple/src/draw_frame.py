import cv2
import pyautogui
import sys

class FrameDrawer:
    def __init__(self, user_input: dict):
        screenwidth, screenheight = pyautogui.size()

        self.screenwidth = screenwidth
        self.screenheight = screenheight

        if self.check_margins(user_input):
            self.start_x = user_input["start_x"]
            self.start_y = user_input["start_y"]

        pyautogui.PAUSE = 0.0

    def check_margins(self, user_input: dict) -> bool:
        start_x = user_input["start_x"]
        start_y = user_input["start_y"]

        if start_x > self.screenwidth:
            sys.exit(f"Start X Of {start_x} Is More Than Screen Width")
        elif start_x < 0:
            sys.exit(f"Start X Of {start_x} Is Less Than Zero")

        if start_y > self.screenheight:
            sys.exit(f"Start Y Of {start_y} Is More Than Screen Height")
        elif start_y < 0:
            sys.exit(f"Start Y Of {start_y} Is Less Than Zero")

        width = user_input["width"]
        height = user_input["height"]

        if width <= 0:
            sys.exit(f"Width Must Be Greater Than Zero")
        elif height <= 0:
            sys.exit(f"Height Must Be Greater Than Zero")

        end_x = start_x + width
        end_y = start_y + height

        if end_x > self.screenwidth:
            sys.exit("End X Of Video Is More Than Screen Width")
        elif end_y > self.screenheight:
            sys.exit("End Y Of Video Is More Than Screen Height")

        return True

    def draw_frame(self, frame):
        ret, thresh = cv2.threshold(frame, 127, 255, cv2.THRESH_BINARY)
        contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

        contours = [cv2.approxPolyDP(c, 1.5, closed=True) for c in contours]

        self.fill_canvas()

        for contour in contours:
            for i, coord in enumerate(contour):
                x = coord[0][0] + self.start_x
                y = coord[0][1] + self.start_y

                if i == 0:
                    pyautogui.moveTo(x, y)

                pyautogui.dragTo(x, y, duration=0.5, button="left")

                i = i + 1

    def fill_canvas(self):
        # Click Rectangle Fill Button
        pyautogui.click(int((self.screenwidth / 2) * 1.7), int(self.screenheight / 2))

        # Click Button To Make Color White
        pyautogui.click(int((self.screenwidth / 2) * 0.51), int((self.screenheight / 2) * 0.83))


        # Drag To Fill Screen
        pyautogui.moveTo(int((self.screenwidth / 2) * 0.7), int((self.screenheight / 2) * 0.58))
        pyautogui.dragTo(int(self.screenwidth * 0.90), int(self.screenheight * 0.90), duration=0.5, button="left")

        # Click Normal Draw Button
        pyautogui.click(int((self.screenwidth / 2) * 1.7), int((self.screenheight / 2) * 0.75))

        # Click Button To Make Color Black
        pyautogui.click(int((self.screenwidth / 2) * 0.51), int((self.screenheight / 2) * 0.73))