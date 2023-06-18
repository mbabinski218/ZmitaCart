import os
import time
from selenium import webdriver
from selenium.webdriver.chrome.options import Options

# Get the current working directory
project_dir = os.getcwd()

# Set the path to the Chrome driver executable
chrome_driver_path = os.path.join(project_dir, "Chrome Driver", "chromedriver.exe")
os.environ["webdriver.chrome.driver"] = chrome_driver_path

options = Options()
driver = webdriver.Chrome(options=options)

driver.get("http://localhost:4200/")

# Add a delay of 3 seconds before the browser closes
time.sleep(3)

# Close the browser window
driver.quit()
