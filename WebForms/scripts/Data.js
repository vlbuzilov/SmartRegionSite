function DeleteImage() {
    const progressBars = document.querySelectorAll('.progress-bar');
    progressBars.forEach(progressBar => {
        const projectId = progressBar.getAttribute('data-project-id');
        const projectImage = document.querySelector(`.project[data-project-id="${projectId}"] img`);

       
        $.ajax({
            url: "/WebForms/CROUDFAUD.aspx/GetProgress",
            type: "POST",
            data: JSON.stringify({ projectId: projectId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {              
                var a = data.d.CurrentProgress;
                var b = data.d.MaxValue;
     
                if (a === b) {
                    projectImage.setAttribute('src', '/WebForms/Images/Frame1.png');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown);
            }
        });
    });
}


setInterval(DeleteImage, 500);


function updateProgressBars() {
    const progressBars = document.querySelectorAll('.progress-bar');
    progressBars.forEach(progressBar => {
        const progress = progressBar.querySelector('.progress');
        const projectId = progressBar.getAttribute('data-project-id');
        fetch('/WebForms/CROUDFAUD.aspx/GetProgress', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ projectId: projectId })
        })
            .then(response => response.json())
            .then(data => {
                var a = data.d.CurrentProgress;
                var b = data.d.MaxValue;
                const percent = (a / b) * 100;
                progress.style.width = `${percent}%`;
            })
            .catch(error => console.error('Failed to update progress.', error));
    });
}
updateProgressBars();
setInterval(updateProgressBars, 10000);