import cv2
import pyautogui

class FrameDrawer():
    def __init__(self):
        screenwidth, screenheight = pyautogui.size()

        self.screenwidth = screenwidth
        self.screenheight = screenheight

    def draw_frame(self, frame):
        pass