import { Component, ViewChild, OnInit, Input, ViewChildren } from '@angular/core';
import { CornerstoneDirective } from './cornerstone.directive';
import { Router } from '@angular/router';
import { ThumbnailDirective } from './thumbnail.directive';
import { faSun } from '@fortawesome/free-solid-svg-icons';
import { faAdjust } from '@fortawesome/free-solid-svg-icons';
import { faScroll } from '@fortawesome/free-solid-svg-icons';
import { faRuler } from '@fortawesome/free-solid-svg-icons';
import { faCircleDot } from '@fortawesome/free-solid-svg-icons';
import { faAngleLeft } from '@fortawesome/free-solid-svg-icons';
import { faCircle } from '@fortawesome/free-solid-svg-icons';
import { faSquare } from '@fortawesome/free-solid-svg-icons';
import { faUndo } from '@fortawesome/free-solid-svg-icons';
import { faSearchPlus } from '@fortawesome/free-solid-svg-icons';
import { faArrowsAlt } from '@fortawesome/free-solid-svg-icons';
import { faCirclePlay } from '@fortawesome/free-solid-svg-icons';
import { faCircleStop } from '@fortawesome/free-solid-svg-icons';
import { faCircleArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { faCircleArrowRight } from '@fortawesome/free-solid-svg-icons';
declare const cornerstone;
declare const cornerstoneTools;

@Component({
  selector: 'webimage-viewer',
  templateUrl: './webimage-viewer.component.html',
  styleUrls: ['./webimage-viewer.component.css']
})


export class WebImageViewerComponent implements OnInit {




  webbaseURL:string="http://localhost:4200"
  //Fa-ICONS
  faSun=faSun;
  faAdjust=faAdjust;
  faScroll=faScroll;
  faRuler=faRuler;
  faAngleLeft=faAngleLeft;
  faCircleDot=faCircleDot
  faCircle=faCircle;
  faSquare=faSquare;
  faUndo=faUndo;
  faSearchPlus=faSearchPlus;
  faArrowsAlt=faArrowsAlt
  faCirclePlay=faCirclePlay;
  faCircleStop=faCircleStop;
  faCircleArrowLeft=faCircleArrowLeft;
  faCircleArrowRight=faCircleArrowRight;

  @Input() public enableViewerTools = false; // enable viewer tools
  @Input() public enablePlayTools = false; // enable Play Clip tools
  @Input() public downloadImagesURL = '' // download images URL
  @Input() public maxImagesToLoad = 10; // limit for the automatic loading of study images

  public seriesList = []; // list of series on the images being displayed
  public currentSeriesIndex = 0;
  public currentSeries: any = {};
  public imageCount = 0; // total image count being viewed
  
  // control enable/disable image scroll buttons
  public get hidePreviousImage(): any { return { color: (this.viewPort.currentIndex < 1) ? 'black' : 'white' }; }
  public get hideNextImage(): any { return { color: (this.viewPort.currentIndex >= (this.imageCount - 1)) ? 'black' : 'white' }; }

  // control message for more images to load
  public get moreImagestoLoad(): string {
    if (this.loadedImages.length < this.imageIdList.length && !this.loadingImages) { // are there any more images to load?
      const imagesToLoad = (this.maxImagesToLoad <= 0) ? (this.imageIdList.length - this.loadedImages.length) : Math.min(this.maxImagesToLoad, this.imageIdList.length - this.loadedImages.length);
      return imagesToLoad.toString();
    } else return '';
  }

  // control exhibition of a loading images progress indicator
  public loadingImages = false;
  public get showProgress(): any { return { display: (this.loadingImages) ? 'inline-block' : 'none' } };

  @ViewChild(CornerstoneDirective, { static: true }) viewPort: CornerstoneDirective; // the main cornertone view port
  @ViewChildren(ThumbnailDirective) thumbnails: Array<ThumbnailDirective>;

  private loadedImages = [];
  private imageIdList = [];
  private element: any;
  private targetImageCount = 0;

  constructor(private router: Router) { }

  ngOnInit() {
    this.element = this.viewPort.element;
  }

  /**
   * Load dicom images for display
   *
   * @param imageIdList list of imageIds to load and display
   */
  loadStudyImages(imageIdList: Array<any>) {
    this.element = this.viewPort.element;
    this.imageIdList = imageIdList;
    this.viewPort.resetViewer();
    this.viewPort.resetImageCache(); // clean up image cache
    this.seriesList = []; // start a new series list
    this.currentSeriesIndex = 0; // always display first series
    this.loadedImages = []; // reset list of images already loaded


    const maxImages = (this.maxImagesToLoad <= 0) ? imageIdList.length : Math.min(this.maxImagesToLoad, imageIdList.length);
    this.loadingImages = true; // activate progress indicator
    this.targetImageCount = maxImages;
    for (let index = 0; index < maxImages; index++) {
      const imageId = imageIdList[index];
      cornerstone.loadAndCacheImage(imageId).then(imageData => { this.imageLoaded(imageData) });
    }

  }

  /**
   * Load the next batch of images
   */
  public loadMoreImages() {
    this.element = this.viewPort.element;
    //
    // loop thru all imageIds, load and cache them for exhibition (up the the maximum limit defined)
    //
    const maxImages = (this.maxImagesToLoad <= 0) ? (this.imageIdList.length - this.loadedImages.length) : Math.min(this.maxImagesToLoad, this.imageIdList.length - this.loadedImages.length);
    this.loadingImages = true; // activate progress indicator
    this.targetImageCount += maxImages;
    let nextImageIndex = this.loadedImages.length;
    for (let index = 0; index < maxImages; index++) {
      const imageId = this.imageIdList[nextImageIndex++];
      cornerstone.loadAndCacheImage(imageId)
        .then(imageData => { this.imageLoaded(imageData) })
        .catch(err => { this.targetImageCount--; });
    }

  }

  /**
   *
   * @param imageData the dicom image data
   */
  private imageLoaded(imageData) {
    // build list of series in all loadded images
    const series = {
      studyID: imageData.data.string('x0020000d'),
      seriesID: imageData.data.string('x0020000e'),
      seriesNumber: imageData.data.intString('x00200011'),
      studyDescription: imageData.data.string('x00081030'),
      seriesDescription: imageData.data.string('x0008103e'),
      imageCount: 1,
      imageList: [imageData]
    }
    // if this is a new series, add it to the list
    let seriesIndex = this.seriesList.findIndex(item => item.seriesID === series.seriesID);
    if (seriesIndex < 0) {
      seriesIndex = this.seriesList.length;
      this.seriesList.push(series);
      this.seriesList.sort((a, b) => {
        if (a.seriesNumber > b.seriesNumber) return 1;
        if (a.seriesNumber < b.seriesNumber) return -1;
        return 0;
      })
    } else {
      let seriesItem = this.seriesList[seriesIndex];
      seriesItem.imageCount++;
      seriesItem.imageList.push(imageData);
      seriesItem.imageList.sort((a, b) => {
        if (a.data.intString('x00200013') > b.data.intString('x00200013')) return 1;
        if (a.data.intString('x00200013') < b.data.intString('x00200013')) return -1;
        return 0;
      })
    }

    this.loadedImages.push(imageData); // save to images loaded

    if (seriesIndex === this.currentSeriesIndex) {
      this.showSeries(this.currentSeriesIndex)
    }

    if (this.loadedImages.length >= this.targetImageCount) { // did we finish loading images?
      this.loadingImages = false; // deactivate progress indicator
    }

  }

  public showSeries(index) {
    this.currentSeriesIndex = index;
    this.currentSeries = this.seriesList[index];
    this.imageCount = this.currentSeries.imageCount; // get total image count
    this.viewPort.resetImageCache(); // clean up image cache
    for (let i = 0; i < this.currentSeries.imageList.length; i++) {
      const imageData = this.currentSeries.imageList[i];
      this.viewPort.addImageData(imageData);
    }

  }

  public saveAs() {
    cornerstoneTools.saveAs(this.element, "teste.jpg")
  }

  /**
   * Image scroll methods
   */
  public nextImage() {
    if (this.viewPort.currentIndex < this.imageCount) {
      this.viewPort.nextImage();
    }
  }

  public previousImage() {
    if (this.viewPort.currentIndex > 0) {
      this.viewPort.previousImage();
    }
  }

  /**
   * Methods to activate/deactivate viewer tools
   */

  // deactivate all tools
  public resetAllTools() {
    if (this.imageCount > 0) {
      this.viewPort.resetAllTools()
      this.stopClip();
    }
  }

  // activate windowing
  public enableWindowing() {
    if (this.imageCount > 0) {
      this.resetAllTools();
      cornerstoneTools.setToolActiveForElement(this.element, 'Wwwc', { mouseButtonMask: 1 }, ['Mouse']);

    }
  }

  // activate zoom
  public enableZoom() {
    if (this.imageCount > 0) {
      this.resetAllTools();
      cornerstoneTools.setToolActiveForElement(this.element, 'Zoom', { mouseButtonMask: 1 }, ['Mouse']); // zoom left mouse
      cornerstoneTools.setToolActiveForElement(this.element, 'Pan', { mouseButtonMask: 2 }, ['Mouse']); // pan right mouse

    }
  }

  // activate pan
  public enablePan() {
    if (this.imageCount > 0) {
      this.resetAllTools();
      cornerstoneTools.setToolActiveForElement(this.element, 'Pan', { mouseButtonMask: 1 }, ['Mouse']);
    }
  }

  // activate image scroll
  public enableScroll() {
    if (this.imageCount > 0) {
      this.resetAllTools();
      cornerstoneTools.setToolActiveForElement(this.element, 'StackScroll', { mouseButtonMask: 1 }, ['Mouse']);
    }
  }

  // activate length measurement
  public enableLength() {
    if (this.imageCount > 0) {
      this.resetAllTools();
      cornerstoneTools.setToolActiveForElement(this.element, 'Length', { mouseButtonMask: 1 }, ['Mouse']);
    }
  }

  // activate angle measurement
  public enableAngle() {
    if (this.imageCount > 0) {
      this.resetAllTools();
      cornerstoneTools.setToolActiveForElement(this.element, 'Angle', { mouseButtonMask: 1 }, ['Mouse']);
    }
  }

  // activate pixel probe
  public enableProbe() {
    if (this.imageCount > 0) {
      this.resetAllTools();
      cornerstoneTools.setToolActiveForElement(this.element, 'Probe', { mouseButtonMask: 1 }, ['Mouse']);
    }
  }

  // activate Elliptical ROI
  public enableElliptical() {
    if (this.imageCount > 0) {
      this.resetAllTools();
      cornerstoneTools.setToolActiveForElement(this.element, 'EllipticalRoi', { mouseButtonMask: 1 }, ['Mouse']);
    }
  }

  // activate Rectangle ROI
  public enableRectangle() {
    if (this.imageCount > 0) {
      this.resetAllTools();
      cornerstoneTools.setToolActiveForElement(this.element, 'RectangleRoi', { mouseButtonMask: 1 }, ['Mouse']);
    }
  }

  // Play Clip
  public playClip() {
    if (this.imageCount > 0) {
      let frameRate = 10;
      let stackState = cornerstoneTools.getToolState(this.element, 'stack');
      if (stackState) {
        frameRate = stackState.data[0].frameRate;
        // Play at a default 10 FPS if the framerate is not specified
        if (frameRate === undefined || frameRate === null || frameRate === 0) {
          frameRate = 10;
        }
      }
      cornerstoneTools.playClip(this.element, frameRate);
    }
  }

  // Stop Clip
  public stopClip() {
    cornerstoneTools.stopClip(this.element);
  }

  // invert image
  public invertImage() {
    if (this.imageCount > 0) {
      let viewport = cornerstone.getViewport(this.element);
      // Toggle invert
      if (viewport.invert === true) {
        viewport.invert = false;
      } else {
        viewport.invert = true;
      }
      cornerstone.setViewport(this.element, viewport);
    }
  }

  // reset image
  public resetImage() {
    if (this.imageCount > 0) {
      let toolStateManager = cornerstoneTools.getElementToolStateManager(this.element);
      // Note that this only works on ImageId-specific tool state managers (for now)
      cornerstoneTools.clearToolState(this.element, "Length");
      cornerstoneTools.clearToolState(this.element, "Angle");
      cornerstoneTools.clearToolState(this.element, "Probe");
      cornerstoneTools.clearToolState(this.element, "EllipticalRoi");
      cornerstoneTools.clearToolState(this.element, "RectangleRoi");
      cornerstone.updateImage(this.element);
      this.resetAllTools();
    }
  }

  logOut() {
    sessionStorage.removeItem('userName');
    sessionStorage.removeItem('userRole');
    sessionStorage.clear();
    this.router.navigate(['']);
  }

  homePage(){
    console.log(sessionStorage.getItem('userRole').toString())
    if(sessionStorage.getItem('userRole').toLowerCase()=="physician"){

      this.router.navigate(['/physician'])
      
    }
    else if(sessionStorage.getItem('userRole').toLowerCase()=="doctor"){
      this.router.navigate(['/doctor'])
    }
    else if(sessionStorage.getItem('userRole').toLowerCase()=="radiationoncologist"){
      this.router.navigate(['radiologist'])
    }
    else if(sessionStorage.getItem('userRole').toLowerCase()=="admin"){
      this.router.navigate(['/admin'])
    }
 }

  public clearImage() {
    this.viewPort.resetViewer();
    this.viewPort.resetImageCache();
    this.seriesList = []; // list of series on the images being displayed
    this.currentSeriesIndex = 0;
    this.currentSeries = {};
    this.imageCount = 0; // total image count being viewed

  }
}
