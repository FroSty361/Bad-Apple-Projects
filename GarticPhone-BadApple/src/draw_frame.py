import cv2
import pyautogui
import sys

class FrameDrawer:
    def __init__(self, video_properties: dict, user_input: dict):
        screenwidth, screenheight = pyautogui.size()

        self.screenwidth = screenwidth
        self.screenheight = screenheight

        if self.check_margins(user_input):
            self.start_x = user_input["start_x"]
            self.start_y = user_input["start_y"]

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

        for contour in contours:
            for coord in contour:
                print(coord[0][0], coord[0][1])