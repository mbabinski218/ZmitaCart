from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys

# Set up the WebDriver instance (in this case, using Firefox)
driver = webdriver.Firefox()

# Navigate to the desired webpage
driver.get("http://localhost:4200/home/offers")

# Find the search input element and enter the search query
search_input = driver.find_element(By.CSS_SELECTOR, 'input[formcontrolname="input"]')
search_query = "CS:GO Klucze"
search_input.send_keys(search_query)

# Submit the search query
search_input.send_keys(Keys.RETURN)

# Wait for the search results page to load (you can add an explicit wait here if needed)
driver.implicitly_wait(5)

# Get the search results and check if the search query is present in any of the results
search_results = driver.find_element(By.CSS_SELECTOR, "p.title.cursor-pointer")

if search_query in search_results.text:
    print("The expected text is present in the search results.")
else:
    print("The expected text is not present in the search results.")

# Close the browser
driver.quit()