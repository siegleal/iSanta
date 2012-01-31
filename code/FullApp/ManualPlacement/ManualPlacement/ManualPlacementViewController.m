//
//  ManualPlacementViewController.m
//  ManualPlacement
//
//  Created by CSSE Department on 1/21/12.
//  Copyright (c) 2012 Rose-Hulman Institute of Technology. All rights reserved.
//

#import "ManualPlacementViewController.h"


@implementation ManualPlacementViewController
@synthesize brain = _brain;
@synthesize tapRecognizer = _tapRecognizer;
@synthesize longPressRec = _longPressRec;
@synthesize imageView = _imageView;


int currentOp = 1;

- (void) viewDidLoad
{
    self.imageView.image = self.brain.targetImage;
    [self.view addGestureRecognizer:[self tapRecognizer]];
    [self.view addGestureRecognizer:[self longPressRec]];
    NSLog(@"Logged");
}

- (PlacementBrain *)brain
{
    if (!_brain) _brain = [[PlacementBrain alloc] init];
    return _brain;
}
- (IBAction)tapped:(id)sender {
    CGPoint loc = [self.tapRecognizer locationInView:self.view];
    NSLog(@"double tap @ %f %f", loc.x,loc.y);
    [self.brain addPointatX:loc.x andY:loc.y];
    
}

- (IBAction)longPress:(id)sender {
    NSLog(@"Long press received");
    //start editing
}




    /* -(void) touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
    {
        //Finger Down
        UITouch *anyTouch = [touches anyObject];
        if (anyTouch.tapCount == 1) 
        {
            //Create a new Smile
            NSLog(@"tapped at %f, %f",[self.tapRecognizer locationInView:self.view].x,[self.tapRecognizer locationInView:self.view].y);  
            
        }else if (anyTouch.tapCount == 2) 
        {
            //Create a new Smile
            NSLog(@"twice");        
        }else if (anyTouch.tapCount == 3) 
        {
            //Create a new Smile
            NSLog(@"thrice");        
        }
    }*/

- (UITapGestureRecognizer *) tapRecognizer{
    if (!_tapRecognizer) _tapRecognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(tapped:)];
    _tapRecognizer.numberOfTapsRequired = 2;
    return _tapRecognizer;
}



- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        return (interfaceOrientation != UIInterfaceOrientationPortraitUpsideDown);
    } else {
        return YES;
    }
}

- (void)viewDidUnload {
    [self setImageView:nil];
    [self setTapRecognizer:nil];
    [self setLongPressRec:nil];
    [super viewDidUnload];
}
@end
