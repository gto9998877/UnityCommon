using System.Collections.Generic;

public class Global {
	public static string loadName;
}


//public class VeeMapData {
//	Dictionary<int, object> _objs = new Dictionary<int, object>();
//
//	public void setObject(object obj,  p) {
//		if (p == null) return;
//		int idx = this.grid2Index(p);
//		if(idx >= 0) {
//			this._objs.Add(idx, obj);
//		}
//	},
//
//	setObjectByIndex : function(obj, index){
//		if(index == null || index == undefined) return;
//		if(index >= 0){
//			this._objs[index] = obj;
//		}
//	},
//
//	getObjectByIndex : function(index){
//		return this._objs[index];
//	},
//
//	/**
//	 *
//	 * @param p {cc.Point|Number}
//	 * @returns {Object}
//	 */
//	getObject : function(p){
//		if (p === null || p === undefined) return null;
//		var idx = p;
//		if (_.isObject(p)) idx = this.grid2Index(p);
//		if(idx >= 0) return this._objs[idx];
//		else return null;
//	},
//
//	/**
//	 * @param p {cc.Point|Number}
//	 */
//	removeObject : function(p){
//		this.setObject(null, p);
//	},
//
//	getObjects : function(){
//		return this._objs;
//	},
//
//	forEachObjects : function(callback) {
//		for (var i in this._objs) {
//			if (this._objs[i] !== null)
//				callback(this._objs[i]);
//		}
//	},
//
//	/**
//	 * @param callback
//	 * @param begin {cc.Point}
//	 * @param fromDir {vee.Direction}
//	 */
//	forEachGrid : function(callback, fromDir) {
//		var fromX, fromY, endX, endY;
//		if (!fromDir) fromDir = vee.Direction.TopLeft;
//		if (vee.Direction.isBottom(fromDir)) {
//			fromY = this.mapSize.height-1;
//			endY = 0;
//		} else {
//			fromY = 0;
//			endY = this.mapSize.height-1;
//		}
//
//		if (vee.Direction.isRight(fromDir)) {
//			fromX = this.mapSize.width-1;
//			endX = 0;
//		} else {
//			fromX = 0;
//			endX = this.mapSize.width-1;
//		}
//		vee.Utils.forEachGrid(cc.p(fromX, fromY), cc.p(endX, endY), callback);
//	},
//
//	removeAllObjects : function(){
//		this._objs = [];
//	},
//
//	/**
//	 * @type cc.Point
//	 */
//	baseLocation : {x:0, y:0},
//
//	/**
//	 * @type cc.Size
//	 */
//	tileSize : {width:0, height:0},
//
//	/**
//	 * @type cc.Size
//	 */
//	mapSize : {width:10, height: 10},
//
//	contentSize : {width:0, height: 0},
//
//	getContentSize : function(){ return cc.size(this.contentSize.width, this.contentSize.height); },
//
//	length : 0,
//	/**
//	 * @param {cc.size} size
//	 */
//	setMapSize : function(size) {
//		this.mapSize = size;
//		this.contentSize.width = this.mapSize.width*this.tileSize.width;
//		this.contentSize.height = this.mapSize.height*this.tileSize.height;
//		this.length = size.width * size.height;
//	},
//
//	/**
//	 * @param {cc.Size} size
//	 */
//	setTileSize : function(size) {
//		this.tileSize = size;
//		this.contentSize.width = this.mapSize.width*this.tileSize.width;
//		this.contentSize.height = this.mapSize.height*this.tileSize.height;
//	},
//
//	/**
//	 * @param {cc.Point} p
//	 */
//	setPosition : function(p) {
//		this.baseLocation = p;
//	},
//	getPosition : function() { return cc.p(this.baseLocation.x, this.baseLocation.y); },
//
//	/**
//	 * @param {cc.Point} p
//	 */
//	position2Grid : function(p){
//		var gridX = Math.floor((p.x - this.baseLocation.x)/this.tileSize.width);
//		var gridY = this.mapSize.height - Math.floor((p.y - this.baseLocation.y)/this.tileSize.height) - 1;
//		if(gridX < 0 || gridY < 0 || gridX >= this.mapSize.width || gridY >= this.mapSize.height) {
//			//			vee.Utils.logObj(cc.p(gridX,gridY), "Bad position2Grid");
//			return null;
//		}
//		return cc.p(gridX,gridY);
//	},
//
//	/**
//	 * @param {cc.Point} p
//	 */
//	mapPosition2Grid : function(p){
//		var gridX = Math.floor(p.x/this.tileSize.width);
//		var gridY = this.mapSize.height - Math.floor(p.y/this.tileSize.height) - 1;
//		if(gridX < 0 || gridY < 0 || gridX >= this.mapSize.width || gridY >= this.mapSize.height) return null;
//		return cc.p(gridX,gridY);
//	},
//
//	/**
//	 * @param {cc.Point} p
//	 * @param {vee.Direction} dir
//	 */
//	gridByDirection : function(p, dir) {
//		return this.lawGrid(cc.pAdd(p, vee.Direction.direction2Point(dir)));
//	},
//
//	/**
//	 * @param {cc.Point} p
//	 */
//	grid2Index : function(p){
//		if(!this.lawGrid(p)) return -1;
//		return p.x + p.y*this.mapSize.width;
//	},
//
//	/**
//	 * @param {Number} idx
//	 */
//	index2Grid : function(idx){
//		return cc.p(idx%this.mapSize.width, Math.floor(idx/this.mapSize.width));
//	},
//
//	/**
//	 * @param {cc.Point} p
//	 */
//	grid2Position : function(p){
//		return cc.p(this.baseLocation.x + (p.x + 0.5)*this.tileSize.width, this.baseLocation.y + (this.mapSize.height - p.y - 0.5)*this.tileSize.height);
//	},
//
//	/**
//	 * @param {cc.Point} p
//	 */
//	grid2PositionInMap : function(p){
//		return cc.p((p.x + 0.5)*this.tileSize.width, (this.mapSize.height - p.y - 0.5)*this.tileSize.height);
//	},
//
//	/**
//	 * @param {cc.Point} p
//	 */
//	forceLawGrid : function(p){
//		var x = Math.max(0,Math.min(this.mapSize.width-1, p.x));
//		var y = Math.max(0,Math.min(this.mapSize.height-1, p.y));
//		return cc.p(x,y);
//	},
//		
//	public bool lawGrid on(p) {
//		if (!p) return null;
//		if (p.x < 0 || p.y < 0 || p.x >= this.mapSize.width || p.y >= this.mapSize.height) {
//			//			vee.Utils.logObj(p, "Bad lawGrid");
//			return null;
//		}
//		return p;
//	}
//});
//
///**
// * @param {cc.Size} mapSize
// * @param {cc.Size} tileSize
// * @returns {vee.Map}
// */
//vee.Map.create = function(mapSize, tileSize) {
//	var map = new vee.Map();
//	if (mapSize) map.setMapSize(mapSize);
//	if (tileSize) map.setTileSize(tileSize);
//	map._objs = [];
//	return map;
//};