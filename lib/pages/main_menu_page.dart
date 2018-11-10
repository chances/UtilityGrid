import 'package:flutter/material.dart';
import 'package:material_design_icons_flutter/material_design_icons_flutter.dart';
import 'package:utility_grid/pages.dart';
import 'package:utility_grid/style.dart';

class MainMenuPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        fit: StackFit.expand,
        alignment: Alignment.center,
        children: <Widget>[
          Center(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: <Widget>[
                Table(
                  defaultColumnWidth: IntrinsicColumnWidth(),
                  border: TableBorder.all(width: 3.0),
                  children: <TableRow>[
                    TableRow(children: <Widget>[
                      Padding(
                        padding: const EdgeInsets.all(Style.marginText),
                        child: Icon(
                          Icons.flash_on,
                          color: Colors.amber,
                          size: 30.0,
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.all(Style.marginText),
                        child: Icon(
                          MdiIcons.signalVariant,
                          color: Colors.black54,
                          size: 30.0,
                        ),
                      ),
                    ]),
                    TableRow(children: <Widget>[
                      Padding(
                        padding: const EdgeInsets.all(Style.marginText),
                        child: Icon(
                          MdiIcons.cityVariant,
                          color: Colors.grey,
                          size: 30.0,
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.all(Style.marginText),
                        child: Icon(
                          MdiIcons.cellphoneBasic,
                          color: Colors.blueGrey,
                          size: 32.0,
                        ),
                      ),
                    ]),
                  ],
                ),
                Padding(
                  padding: const EdgeInsets.only(top: Style.margin),
                  child: Text(
                    'Utility Grid',
                    style:
                        TextStyle(fontSize: 20.0, fontWeight: FontWeight.bold),
                  ),
                ),
                SizedBox(
                  height: 30.0,
                ),
                RaisedButton.icon(
                  onPressed: () => Navigator.of(context).push(MaterialPageRoute(
                        builder: (BuildContext context) => GamePage(),
                      )),
                  icon: Icon(Icons.play_arrow),
                  label: Text('Start a New Game'),
                ),
                RaisedButton.icon(
                  onPressed: null,
                  icon: Icon(Icons.save),
                  label: Text('Load a Game'),
                ),
              ],
            ),
          )
        ],
      ),
    );
  }
}
