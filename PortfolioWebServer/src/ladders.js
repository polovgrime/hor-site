class Ladder {
  constructor() {
    this.ladderCanvas = document.createElement('canvas');
    this.ctx = ladderCanvas.getContext('2d');
    this.ladder = new Image();
    this.ladder.src = "./img/ladder.png";
    this.topOffset = ladderCanvas.clientHeight / 6;
    this.bigRowHeight = topOffset / 5;
    this.smallRowHeight = bigRowHeight / 3;
    this.smallRowsCount = topOffset / smallRowHeight;
    this.bigRowCount = (topOffset / bigRowHeight) * 3;
    window.requestAnimationFrame(draw);
  }

  draw() { 
    this.ctx.globalCompositeOperation = "destination-over";
    this.ctx.clearRect(0, 0, this.ladderCtx.clientWidth, this.ladderCtx.clientHeight); // clear canvas

    drawLadders(65);

    for (let i = 1; i < this.smallRowsCount + this.bigRowCount; i++) {
      drawLine(i)
    }

    this.ctx.beginPath();
    this.ctx.strokeStyle = "rgba(235, 245, 255, 1)";
    this.ctx.fillRect(0, 0, this.ladderCanvas.clientWidth, this.ladderCanvas.clientHeight);
    this.ctx.stroke();
    this.ctx.closePath();
    window.requestAnimationFrame(draw);
  }

  drawLine(row) {
    let rowHeight = this.bigRowHeight;
    if (row < this.smallRowsCount) {
      rowHeight = this.smallRowHeight;
    }
    let offset = this.topOffset;

    if (row > this.smallRowsCount) {
      offset += this.topOffset;
      offset += (row - this.smallRowsCount) *  this.bigRowHeight;
    } else {
      offset += row *  this.smallRowHeight;
    }

    this.ctx.beginPath(); 
    this.ctx.strokeStyle =  this.getColor(row);
    this.ctx.rect(0, offset,  this.ladderCanvas.clientWidth, rowHeight);
    this.ctx.lineWidth = rowHeight;
    this.ctx.closePath();
    this.ctx.stroke();
  }

  getColor(row) {
    let step;
    if (row > this.smallRowsCount) {
      step = 255 - (255 / this.bigRowCount) * (row - this.smallRowsCount);
    } else {
      step = (255 / this.smallRowsCount) * row;
    }

    if (step == 0) {
      step = 10;
    }

    if (step == 255) {
      step = 245;
    }

    return `rgba(${step - 10}, ${step}, ${step + 10}, 1)`;
  }

  drawLadders(laddersNum) {
    const spaceBetween = this.ctx.canvas.clientWidth / laddersNum;

    let xOffset = spaceBetween;

    for (let i = 0; i < laddersNum; i++) {
      this.ctx.save()
      this.ctx.translate(xOffset, this.ctx.canvas.clientHeight / 2 - 50);
      this.ctx.rotate(getRotation(i) + getRotationOffset());
      this.ctx.drawImage(ladder, 0, 0, 32, 32);
      xOffset += spaceBetween;
      this.ctx.restore();
    }
  }

  getCanvas() {
    return this.ladderCanvas;
  }
}

function fillArray(first, last) {
  let array = [];
  for (let i = first; i <= last; i++) {
    array.push(i);
  }
  return array;
}

function fillArray(first, last) {
  let array = [];
  for (let i = first; i <= last; i++) {
    array.push(i);
  }
  return array;
}

const rotations = fillArray(45, 135);

function getRotationOffset(){
  if (random(0, 100) > 90) {
    return inRad(random(0, new Date().getSeconds() % 5));
  }

  return 0;
}

function getRotation() {
  return inRad(rotations[random(0, rotations.length)]);
}

function random(min, max) {
  return Math.floor(Math.random() * max) + min;
}

function inRad(num) {
  return num * Math.PI / 180;
}

function getRotation(index) {
  if (index == 0) return rotations[0];
  return rotations[index % rotations.length];
}
