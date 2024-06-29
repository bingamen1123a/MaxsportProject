//Active link
const hrefLocation1 = location.href;
const menuItem1 = document.querySelectorAll('.act');
const menuLength1 = menuItem1.length;
for (let i = 0; i < menuLength1; i++) {
    if (menuItem1[i].href === hrefLocation1) {
        menuItem1[i].classList.add("active");
    }
}