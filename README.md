In this project, I created an array of two "source warehouses" UC_From UserControls.
Each UserControl contains an array of controls Control[] arrControls with 30 elements, where:
The arrControls array is one-dimensional and contains 30 Label controls.
Each control has a fixed size, random text "1" or "2", and a random shade of red, green, or blue background color.
There are "destination warehouses" with the titles "Red To", "Green To", and "Blue To".
Initially, the destination warehouses are empty (i.e., all elements of the arrControls array have white background and empty text).

Goal
Transfer all "boxes" from the UC_From source warehouse to the arrUC_To destination warehouses, where:
Each destination warehouse receives only controls of the color according to the text that appears in their titles, for example "Red To" receives controls with a red hue.
The order of transferring the controls is:
First, controls with the text "1" are transferred. When "1" is finished, we move on to the text "2".
The transfer of colors is done in a queue, where only 5 controls of the same color are transferred at a time and then we move on to the next color.
The solution is based on 3 parallel threads.
