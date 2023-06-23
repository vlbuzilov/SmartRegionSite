function toggleDescription() {
    var shortDesc = document.getElementById('shortDesc');
    var longDesc = document.getElementById('longDesc');
    var readMore = document.getElementsByTagName('a')[0];

    if (shortDesc.style.display === 'none') {
        shortDesc.style.display = 'inline';
        longDesc.style.display = 'none';
        readMore.innerText = 'Read More';
    } else {
        shortDesc.style.display = 'none';
        longDesc.style.display = 'inline';
        readMore.innerText = 'Read Less';
    }
}